using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using ShorkBot.Commands;
using ShorkBot.Data;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;


namespace ShorkBot
{
    public class Bot : BackgroundService
    {
        private readonly TwitchClient _client;
        private readonly BotConfiguration _botConfiguration;
        
        public Bot()
        {
            _botConfiguration = BotConfiguration.LoadConfigAsync("config.json").ConfigureAwait(false).GetAwaiter().GetResult();
            var credentials = new ConnectionCredentials(_botConfiguration.Username, _botConfiguration.Token);
            
            var clientOptions = new ClientOptions
            {   
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(60)
            };

            var wsc = new WebSocketClient(clientOptions);
            _client = new TwitchClient(wsc);
            
            _client.Initialize(credentials, _botConfiguration.Channel);
            _client.Connect();
        }
        
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _botConfiguration.CancellationToken = cancellationToken;
                
                _client.OnConnected += OnConnected;
                _client.OnMessageReceived += OnMessageReceived;

            }, cancellationToken).ConfigureAwait(false);
        }
        
        private async void OnConnected(object sender, OnConnectedArgs e)
        {
            if (sender != null)
            {
                await Task.Run( () =>
                {
                    Log.Information("Successfully Connected");
                    Log.Information($"Channel: {_botConfiguration.Channel}");
                    Log.Information($"User: {e.BotUsername}");

                }, _botConfiguration.CancellationToken).ConfigureAwait(false);
            }
        }
        
        private async void OnMessageReceived(object sender, OnMessageReceivedArgs eventType)
        {
            if (sender != null)
            {
                await Task.Run(async () =>
                {
                    var argPos = 0;
                    if (eventType.ChatMessage.Message.HasStringPrefix("-", ref argPos) || 
                        eventType.ChatMessage.Message.HasMentionPrefix(ref argPos, _botConfiguration))
                    {
                        var messageContents = eventType.ChatMessage.Message.TrimPrefix(argPos);
                        var commandName = messageContents.Split(' ')[0];
                        
                        var commandService = new CommandService(new GeneralCommands(_client, _botConfiguration));
                        await commandService.ExecuteAsync(commandName, messageContents, eventType);
                    }
                    
                }, _botConfiguration.CancellationToken).ConfigureAwait(false);
            }
        }
    }
}