using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
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
        public bool IsFullscreen { get; set; }
        public string EditorPosition { get; set; } = "col-md-6";

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
                IsTaskPassed = _taskPlaygroundViewModel.IsPassed ? 1 : 0,
                Solution = _taskPlaygroundViewModel.CopyOfLastExecutedVersion,
                TaskId = Guid
            };

            await HttpClient.PostAsJsonAsyncJwtHeader(localStorageService, submitTaskData, "tasks/history/submittask");

            await _tasksHistoryDraft.RemoveDraft(Guid);
            CantSubmit = false;
        }

        public void SwitchFullscreen()
        {
            IsFullscreen = !IsFullscreen;
            EditorPosition = IsFullscreen ? "col-md-12" : "col-md-6";
        }

        public void ChangeEditorPosition() => EditorPosition = EditorPosition == "col-md-6" ? "col-md-12" : "col-md-6";

        public void GoToTaskForum(Guid taskId)
        {
            NavigationManager.NavigateTo($"/forum/{taskId}");
        }

        void IDisposable.Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}

