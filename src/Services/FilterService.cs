using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace ShorkBot.Services
{
    public class FilterService : BackgroundService
    {
        private readonly TwitchClient _client;
        private readonly BotConfiguration _botConfiguration;

        public FilterService(TwitchClient client, BotConfiguration botConfiguration)
        {
            _client = client;
            _botConfiguration = botConfiguration;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                _botConfiguration.CancellationToken = stoppingToken;

                _client.OnMessageReceived += OnMessageReceived;
            }, stoppingToken).ConfigureAwait(false);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs eventType)
        {
            if (ContainsBlockedWord(eventType.ChatMessage.Message) /*&& !eventType.ChatMessage.UserType.HasElevatedPermissions()*/)
            {
                Log.Information(eventType.ChatMessage.Message);
                _client.DeleteMessage(_botConfiguration.Channel, eventType.ChatMessage);
                return;
            }
        }
        
        private bool ContainsBlockedWord(string contents)
        {
            return _botConfiguration.Blocked.Any(str => new Regex($".*{str}.*").IsMatch(contents.ToLowerInvariant()));
        }
    }
}