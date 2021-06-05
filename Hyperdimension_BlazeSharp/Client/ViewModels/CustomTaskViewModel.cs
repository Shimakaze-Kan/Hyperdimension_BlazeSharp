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

        public IEnumerable<ModuleWithTasks> Modules { get; set; }
        public CustomModuleCreateRequest ModuleCreateRequest { get; set; }
        public TaskCreateRequest TaskCreateRequest { get; set; }

        public CustomTaskViewModel(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task CreateModule()
        {
            await _httpClient.PostAsJsonAsyncJwtHeader(_localStorageService, ModuleCreateRequest, "modules");
        }

        public Task CreateTask()
        {
            throw new NotImplementedException();
        }

        public async Task GetModules(int mode)
        {
            Modules = await _httpClient.GetFromJsonAsync<IEnumerable<ModuleWithTasks>>($"modules/mode/{mode}");
        }
    }
}
