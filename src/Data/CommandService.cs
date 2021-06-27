using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ShorkBot.Commands;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace ShorkBot.Data
{
    public class CommandService
    {
        private readonly GeneralCommands _generalCommands;

        public CommandService(GeneralCommands generalCommands)
        {
            _generalCommands = generalCommands;
        }

        public async Task ExecuteAsync(string commandName, string messageContents, OnMessageReceivedArgs eventType) 
        {
            messageContents = messageContents.TrimCommandName();
            
            switch (commandName)
            {
                case "say":
                    await _generalCommands.Say(messageContents);
                    break;
                case "clear":
                    await _generalCommands.Clear(eventType);
                    break;
                case "roll":
                    if (int.TryParse(string.Join("", messageContents.Where(char.IsDigit)), out var count))
                    {
                        await _generalCommands.Dice(count);
                    }
                    else
                    {
                        _generalCommands.Client.SendMessage(_generalCommands.DefaultChannel, "Invalid number");
                    }
                    break;
                default:
                    _generalCommands.Client.SendMessage(_generalCommands.DefaultChannel, $"Command [{commandName}] does not exist.");
                    break;
            }
        }

        public async Task<List<string>> AddModulesAsync(Assembly assembly, IServiceProvider services)
        {
            var types = assembly.ExportedTypes.Where(t => t.BaseType == typeof(ModuleBase<TwitchClient>));
            var modules = types.GetType().GetCustomAttributes<CommandAttribute>();

            return null;
        }
    }
}