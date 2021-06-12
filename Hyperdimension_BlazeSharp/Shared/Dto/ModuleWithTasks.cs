using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record ModuleWithTasks : ModuleMinimal
    {
        public IEnumerable<TaskMinimal> Tasks { get; init; }
        public Guid Id { get; init; }

        public ModuleWithTasks(Guid id, string title, IEnumerable<TaskMinimal> tasks) 
            : base(title) => (Id, Tasks) = (id, tasks);
    }
}
