using System;
using System.Threading.Tasks;
using ShorkBot.Data;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace ShorkBot.Commands
{
    [Module("General Commands")]
    public class GeneralCommands : ModuleBase<TwitchClient>
    {
        public GeneralCommands(TwitchClient client, BotConfiguration botConfiguration) : base(client, botConfiguration)
        {
        }

        [Command("say")]
        public async Task Say(string message)
        {
            await ReplyAsync(message).ConfigureAwait(false);
        }

        [Command("clear")]
        public async Task Clear(OnMessageReceivedArgs type)
        {
            if (!type.ChatMessage.UserType.HasElevatedPermissions())
            {
                return;
            }

            await ClearAsync();
        }

        [Command("discord")]
        public async Task Discord()
        {
            await ReplyAsync("https://discord.com/invite/3qAtRfp").ConfigureAwait(false);
        }

        [Command("roll")]
        public async Task Dice(int count = 6)
        {
            var rand = new Random();
            var roll = rand.Next(1, count + 1);

            await ReplyAsync($"You rolled a {roll}").ConfigureAwait(false);
        }

    }
}