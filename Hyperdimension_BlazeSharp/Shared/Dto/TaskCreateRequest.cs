using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class TaskCreateRequest
    {
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string InitialCode { get; set; }
        public string TestCode { get; set; }
        [Required]
        public int? Points { get; set; }

        #region dumb converters
        public string StringToModuleId
        {
            set => ModuleId = Guid.Parse(value);
            get => ModuleId.ToString();
        }

        public string StringToPoints
        {
            set => Points = Convert.ToInt32(value);
            get => Points.ToString();
        }
        #endregion
    }
}
