using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class LearningPathViewModel : ILearningPathViewModel
    {
        public int ModeNumber { get; set; }
        public IEnumerable<ModuleWithTasks> ModulesWithTasks { get; set; }
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ILocalStorageService _localStorageService;

        public LearningPathViewModel() { }
        public LearningPathViewModel(HttpClient httpClient, IJSRuntime jsRuntime, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _localStorageService = localStorageService;
        }

        public async Task GetModulesWithTasks()
        {
            var modulesWithTasks = await _httpClient.GetFromJsonAsync<IEnumerable<ModuleWithTasks>>($"modules/mode/{ModeNumber}");
            LoadCurrentObject(modulesWithTasks.ToList());
        }
        public async Task DeleteTask(Guid id)
        {
            var state = await _jsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this task ? This operation is irreversible.");

            if(state)
            {                
                await _httpClient.DeleteAsyncJwtHeader(_localStorageService, $"tasks/{id}");
                await GetModulesWithTasks();
            }
        }

        private void LoadCurrentObject(LearningPathViewModel learningPathViewModel) 
            => this.ModulesWithTasks = learningPathViewModel.ModulesWithTasks;

        public async Task DeleteModule(Guid id)
        {
            var state = await _jsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this module ? This operation is irreversible.");

            if (state)
            {
                await _httpClient.DeleteAsyncJwtHeader(_localStorageService, $"modules/{id}");
                await GetModulesWithTasks();
            }
        }

        public static implicit operator LearningPathViewModel(List<ModuleWithTasks> moduleWithTasks)
            => new() { ModulesWithTasks = moduleWithTasks};
    }
}
