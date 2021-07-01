using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class SubcommentCreateRequest
    {
        public Guid MainCommentId { get; set; }
        [MinLength(5, ErrorMessage = "Comment have to be at least 5 characters long")]
        public string Text { get; set; }
    }
}
