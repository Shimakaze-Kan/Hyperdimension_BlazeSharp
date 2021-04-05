using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record TaskDataPlayground : TaskMinimalWithPoints
    {
        public string Description { get; }
        public string InitialCode { get; }
        public string TestCode { get; }

        public TaskDataPlayground(Guid guid, string title, int? points, string description, string initialCode, string testCode)
            : base(guid, title, points) => (Description, InitialCode, TestCode) = (description, initialCode, testCode);

    }
}
