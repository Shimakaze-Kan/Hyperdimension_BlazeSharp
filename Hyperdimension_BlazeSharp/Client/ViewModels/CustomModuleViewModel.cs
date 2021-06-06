using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class CustomModuleViewModel : ICustomModuleViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public CustomModuleCreateRequest CreateRequest { get; set; } = new();

        public CustomModuleViewModel(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<HttpResponseMessage> CreateModule()
        {
            return await _httpClient.PostAsJsonAsyncJwtHeader(_localStorageService, CreateRequest, "modules");
        }
    }
}
