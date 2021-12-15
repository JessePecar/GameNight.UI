using GameNight.Core.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameNight.Core.Http
{
    public class GameHttpClient : IGameHttpClient
    {
        private readonly HttpClient _httpClient;
        public GameHttpClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("GameNight");

            //SETUP Default Headers. 

            _httpClient.Timeout = new TimeSpan(hours: 0, minutes: 1, seconds: 30);
        }

        public async Task<TGet> GetAsync<TGet>(string controller, string action, Dictionary<string, string> paramList)
        {
            string url = BuildUrl(controller, action, paramList);
            HttpResponseMessage response = await _httpClient.SendAsync(GetRequestMessage(HttpMethod.Get, url));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TGet>(await response.Content.ReadAsStringAsync());
            }

            return default(TGet);
        }

        public async Task<TGet> PostAsync<TGet, TPost>(string controller, string action, TPost postObject, Dictionary<string, string> paramList)
        {
            HttpResponseMessage response = await PostAsync(controller, action, postObject, paramList);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    return JsonConvert.DeserializeObject<TGet>(responseContent);
                }
                return default(TGet);
            }

            return default(TGet);
        }

        public async Task<bool> PostForResponseAsync<TPost>(string controller, string action, TPost postObject, Dictionary<string, string> paramList)
        {
            HttpResponseMessage response = await PostAsync(controller, action, postObject, paramList);

            return response.IsSuccessStatusCode;
        }

        private async Task<HttpResponseMessage> PostAsync<TPost>(string controller, string action, TPost postObject, Dictionary<string, string> paramList)
        {
            string url = BuildUrl(controller, action, paramList);
            return await _httpClient.SendAsync(GetRequestMessage(HttpMethod.Post, url, postObject));
        }

        private HttpRequestMessage GetRequestMessage(HttpMethod httpMethod, string url)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = new Uri(url)
            };
            return requestMessage;
        }

        private HttpRequestMessage GetRequestMessage<TContent>(HttpMethod httpMethod, string url, TContent content)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };
            return requestMessage;
        }

        private string BuildUrl(string controller, string action, Dictionary<string, string> paramList)
        {
            string url = string.Empty;
            if (paramList == null)
            {
                url = $"{_httpClient.BaseAddress.AbsoluteUri}api/{controller}/{action}";
            }
            else
            {
                url = $"{_httpClient.BaseAddress.AbsoluteUri}api/{controller}/{action}?{string.Join('&', paramList.Select(pl => $"{pl.Key}={pl.Value}"))}";
            }

            return url;
        }
    }
}
