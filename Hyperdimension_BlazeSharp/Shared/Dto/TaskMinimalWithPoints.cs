using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record TaskMinimalWithPoints : TaskMinimal
    {
        public int? Points { get; init; }

        public TaskMinimalWithPoints(Guid id, string title, int? points)
            : base(id, title) => Points = points;
    }
}
