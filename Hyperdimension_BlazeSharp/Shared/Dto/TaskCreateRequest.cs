﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class TaskCreateRequest
    {
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InitialCode { get; set; }
        public string TestCode { get; set; }
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
