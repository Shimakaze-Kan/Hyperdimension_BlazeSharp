using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared
{
    public class Task
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InitialCode { get; set; }
        public string TestCode { get; set; }
        public int Points { get; set; }
    }
}
