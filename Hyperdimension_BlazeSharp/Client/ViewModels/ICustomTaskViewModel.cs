using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface ICustomTaskViewModel
    {
        public IEnumerable<ModuleItemMinimal> Modules { get; set; }
        public TaskCreateRequest TaskCreateRequest { get; set; }

        public Task GetModules();        
        public Task<HttpResponseMessage> CreateTask();
    }
}
