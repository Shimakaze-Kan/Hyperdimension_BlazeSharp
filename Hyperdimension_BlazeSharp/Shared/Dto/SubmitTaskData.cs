using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class SubmitTaskData
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public string Solution { get; set; }
        public sbyte IsTaskPassed { get; set; }
    }
}
