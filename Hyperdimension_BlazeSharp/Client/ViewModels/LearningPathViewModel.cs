using Hyperdimension_BlazeSharp.Shared.Dto;
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

        public LearningPathViewModel() { }
        public LearningPathViewModel(HttpClient httpClient) => _httpClient = httpClient;

        public async Task GetModulesWithTasks()
        {
            var modulesWithTasks = await _httpClient.GetFromJsonAsync<IEnumerable<ModuleWithTasks>>($"modules/mode/{ModeNumber}");
            LoadCurrentObject(modulesWithTasks.ToList());
        }

        private void LoadCurrentObject(LearningPathViewModel learningPathViewModel) 
            => this.ModulesWithTasks = learningPathViewModel.ModulesWithTasks;

        public static implicit operator LearningPathViewModel(List<ModuleWithTasks> moduleWithTasks)
            => new() { ModulesWithTasks = moduleWithTasks};
    }
}
