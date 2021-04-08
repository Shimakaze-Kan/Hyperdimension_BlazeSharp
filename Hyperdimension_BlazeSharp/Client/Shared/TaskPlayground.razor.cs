using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.Shared
{
    public partial class TaskPlayground
    {
        [Parameter] public Guid Guid { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _taskPlaygroundViewModel.TaskId = Guid;
            await _taskPlaygroundViewModel.GetTask();
            _taskPlaygroundViewModel.CheckIfDraftExists();
        }
    }
}
