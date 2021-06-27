using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;

namespace ShorkBot.Data
{
    public class ModuleBase<T> where T : TwitchClient
    {
        public TwitchClient Client { get; private set; }
        public BotConfiguration BotConfiguration { get; private set; }
        public string DefaultChannel => BotConfiguration.Channel;

        public ModuleBase(TwitchClient client, BotConfiguration botConfiguration)
        {
            Client = client;
            BotConfiguration = botConfiguration;
        }
        
        protected virtual async Task ReplyAsync(string message)
        {
            await Task.Run(() =>
            {
                Client.SendMessage(DefaultChannel, message);
            }, BotConfiguration.CancellationToken).ConfigureAwait(false);
        }

        protected virtual async Task ReplyAsync(string channel, string message)
        {
            await Task.Run(() =>
            {
                Client.SendMessage(channel, message);
            }, BotConfiguration.CancellationToken).ConfigureAwait(false);
        }

        protected virtual async Task ClearAsync()
        {
            await Task.Run(() =>
            {
                Client.ClearChat(DefaultChannel);
            }, BotConfiguration.CancellationToken).ConfigureAwait(false);
        }

        protected virtual async Task ClearAsync(string channel)
        {
            await Task.Run(() =>
            {
                Client.ClearChat(DefaultChannel);
            }, BotConfiguration.CancellationToken).ConfigureAwait(false);
        }
    }
}