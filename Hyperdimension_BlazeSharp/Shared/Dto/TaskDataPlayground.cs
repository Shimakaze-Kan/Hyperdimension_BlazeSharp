using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record TaskDataPlayground : TaskMinimalWithPoints
    {
        public string Description { get; init; }
        public string InitialCode { get; init; }
        public string TestCode { get; init; }
        public int Mode { get; init; }

        public TaskDataPlayground(Guid guid, string title, int? points, string description, string initialCode, string testCode, int mode)
            : base(guid, title, points) => (Description, InitialCode, TestCode, Mode) = (description, initialCode, testCode, mode);

    }
}
