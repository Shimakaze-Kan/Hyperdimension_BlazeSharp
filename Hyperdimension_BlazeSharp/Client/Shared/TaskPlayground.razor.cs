using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.Shared
{
    public partial class TaskPlayground
    {
        [CascadingParameter]
        public Task<AuthenticationState> authenticationState { get; set; }
        [CascadingParameter(Name = "_tasksHistoryDraft")]
        TasksHistoryDraft _tasksHistoryDraft { get; set; }
        [Parameter] public Guid Guid { get; set; }
        [Inject] public ILocalStorageService localStorageService { get; set; }

        public bool CantSubmit { get; set; }

        protected async override Task OnInitializedAsync()
        {
            CantSubmit = !(await authenticationState).User.Identity.IsAuthenticated;
            _taskPlaygroundViewModel.TaskId = Guid;
            await _taskPlaygroundViewModel.GetTask();

            NavigationManager.LocationChanged += LocationChanged;
        }

        async void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            _taskPlaygroundViewModel.TaskId = Guid;
            await _taskPlaygroundViewModel.GetTask();
        }

        public async Task SubmitTask()
        {
            CantSubmit = true;

            var submitTaskData = new SubmitTaskData()
            {
                IsTaskPassed = 1,
                Solution = await _taskPlaygroundViewModel.GetValue(),
                TaskId = Guid
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "tasks/history/submittask")
            {
                Content = JsonContent.Create(submitTaskData)
            };
            //await HttpClient.PostAsJsonAsync<SubmitTaskData>("tasks/history/submittask", submitTaskData);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await localStorageService.GetItem<string>("hbsToken"));
            await HttpClient.SendAsync(request);

            await _tasksHistoryDraft.RemoveDraft(Guid);
            CantSubmit = false;
        }

        void IDisposable.Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}

