using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.Shared
{
    public partial class TaskPlayground
    {
        [Parameter] public Guid Guid { set => _taskPlaygroundViewModel.TaskId = value; }
    }
}
