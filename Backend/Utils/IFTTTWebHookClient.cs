using Newtonsoft.Json;
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

        public void Connect(string token)
        {
            Token = token;
        }

        public void FireEvent(string eventName, dynamic args)
        {
            string finalURL = "https://maker.ifttt.com/";
            if (Token != null) {
                using (var client = new HttpClient()) {
                    client.BaseAddress = new Uri(finalURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8, "application/json");

                    Task.Run(async () => { await client.PostAsync($"trigger/{eventName}/with/key/{Token}", content); }).Wait();
                }
            }
        }
    }
}
