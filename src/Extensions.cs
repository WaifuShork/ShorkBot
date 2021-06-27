using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchLib.Client;
using TwitchLib.Client.Enums;

namespace ShorkBot
{
    public static class Extensions
    {
        public static bool HasStringPrefix(
            this string msg,
            string str,
            ref int argPos,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(msg) || !msg.StartsWith(str, comparisonType))
                return false;
            argPos = str.Length;
            return true;
        }

        public static bool HasMentionPrefix(this string msg, ref int argPos, BotConfiguration configuration)
        {
            if (string.IsNullOrEmpty(msg) || msg.Length <= 3 || msg[0] != '@')
            {
                return false;
            }
            
            var num = msg.IndexOf(' ', StringComparison.Ordinal);
            
            if (num == -1 || msg.Length < num + 2)
            {
                return false;
            }

            argPos = num + 1;
            return true;
        }
        
        public static string TrimPrefix(this string msg, int argPos)
        {
            return msg[argPos..];
        }
        
        public static string TrimCommandName(this string message)
        {
            var contents = message.Split(' ').ToList();
            contents.RemoveAt(0);
            var sb = new StringBuilder();
            foreach (var content in contents)
            {
                sb.Append($"{content} ");
            }

            return sb.ToString();
        }
        
        public static bool HasElevatedPermissions(this UserType userType)
        {
            return userType switch
            {
                UserType.Viewer => false,
                UserType.Moderator => true,
                UserType.GlobalModerator => true,
                UserType.Broadcaster => true,
                UserType.Admin => true,
                UserType.Staff => true,
                _ => false
            };
        }
        
    }
}