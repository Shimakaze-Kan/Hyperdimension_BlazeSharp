using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jSRuntime) => _jsRuntime = jSRuntime;


        public async Task<T> GetItem<T>(string key)
        {
            var jsonItem = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (jsonItem is null)
                return default;

            return JsonSerializer.Deserialize<T>(jsonItem);
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
    }
}
