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
    public class CustomTaskViewModel : ICustomTaskViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public IEnumerable<ModuleItemMinimal> Modules { get; set; }
        public TaskCreateRequest TaskCreateRequest { get; set; } = new();

        public CustomTaskViewModel(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task CreateTask()
        {
            await _httpClient.PostAsJsonAsyncJwtHeader(_localStorageService, TaskCreateRequest, "tasks");
        }

        public async Task GetModules()
        {
            Modules = await _httpClient.GetFromJsonAsync<IEnumerable<ModuleItemMinimal>>("modules");
        }
    }
}
