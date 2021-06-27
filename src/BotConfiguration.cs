using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using TwitchLib.Api.V5.Models.Users;
using TwitchLib.Client.Models;

namespace ShorkBot
{
    public class BotConfiguration
    {
        [JsonProperty("username")]
        public string Username { get; private set; }
        
        [JsonProperty("token")]
        public string Token { get; private set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        
        [JsonProperty("channel")]
        public string Channel { get; private set; }
        
        [JsonProperty("blocked")]
        public List<string> Blocked { get; private set; }
        
        public CancellationToken CancellationToken { get; set; }
        
        public static async Task<BotConfiguration> LoadConfigAsync(string file)
        {
            try
            {
                var contents = await File.ReadAllTextAsync(file).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<BotConfiguration>(contents);
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to deserialize json file");
            }

            return null;
        }
    }
}