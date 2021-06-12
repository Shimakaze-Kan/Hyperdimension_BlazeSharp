using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface ILearningPathViewModel
    {
        public int ModeNumber { get; set; }
        public IEnumerable<ModuleWithTasks> ModulesWithTasks { get; set; }

        public Task GetModulesWithTasks();
        public Task DeleteTask(Guid id);
        public Task DeleteModule(Guid id);
    }
}
