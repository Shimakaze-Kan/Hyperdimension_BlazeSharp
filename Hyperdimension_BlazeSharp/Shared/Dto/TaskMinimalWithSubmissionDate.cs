using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record TaskMinimalWithSubmissionDate : TaskMinimal
    {
        public DateTime SubmittedAt { get; }

        public TaskMinimalWithSubmissionDate(Guid guid, string title, DateTime submittedAt)
            : base(guid, title) => SubmittedAt = submittedAt;
    }
}
