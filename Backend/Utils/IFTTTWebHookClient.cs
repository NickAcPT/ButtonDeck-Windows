using Newtonsoft.Json;
using NickAc.Backend.Utils.Misc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Utils
{
    public class IFTTTWebHookClient
    {
        public string Token { get; set; }

        public IFTTTWebHookClient WithToken(string token)
        {
            Token = token;
            return this;
        }

        public bool IsConnected {
            get => IsValid(Token);
        }

        public static bool IsValid(string token) => token != null && !string.IsNullOrEmpty(token.Trim());

        public void FireEvent(string eventName, IFTTTWebhookProperties args)
        {
            string baseURL = "https://maker.ifttt.com/";
            if (Token != null) {
                using (var client = new HttpClient()) {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(args != null ? args.ToJson() : "", Encoding.UTF8, "application/json");

                    Task.Run(async () => { await client.PostAsync($"trigger/{eventName}/with/key/{Token}", content); }).Wait();
                }
            }
        }
    }
}
