using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface ICustomTaskViewModel
    {
        public IEnumerable<ModuleWithTasks> Modules { get; set; }
        public CustomModuleCreateRequest ModuleCreateRequest { get; set; }
        public TaskCreateRequest TaskCreateRequest { get; set; }

        public Task GetModules(int mode);
        public Task CreateModule();
        public Task CreateTask();
    }
}
