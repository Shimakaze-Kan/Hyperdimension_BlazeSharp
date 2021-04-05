using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record ModuleWithTasks : ModuleMinimal
    {
        public IEnumerable<TaskMinimal> Tasks { get; set; }

        public ModuleWithTasks(string title, IEnumerable<TaskMinimal> tasks) 
            : base(title) => Tasks = tasks;
    }
}
