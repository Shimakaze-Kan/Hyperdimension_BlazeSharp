using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ExtensionMethods
{
    public static class HttpClientExtension
    {
        public static async Task<T> GetJsonAsyncJwtHeader<T>(this HttpClient httpClient, ILocalStorageService localStorageService, string url)
        {
            var token = await localStorageService.GetItem<string>("hbsToken");
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            var reader = new StreamReader(new MemoryStream(responseBytes), Encoding.Default);
            return new Newtonsoft.Json.JsonSerializer().Deserialize<T>(new JsonTextReader(reader));
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsyncJwtHeader<T>(this HttpClient httpClient, ILocalStorageService localStorageService, T data, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(data)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await localStorageService.GetItem<string>("hbsToken"));
            return await httpClient.SendAsync(request);
        }
    }
}
