using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class CommentCreateRequest
    {
        public Guid TaskId { get; set; }
        public string Text { get; set; }
        public bool AddLastSubmittedVersion { get; set; }
    }
}
