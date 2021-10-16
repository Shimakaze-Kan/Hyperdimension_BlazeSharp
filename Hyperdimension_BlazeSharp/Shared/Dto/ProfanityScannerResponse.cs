using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class ProfanityScannerResponse
    {
        public bool IsInappropriate { get; set; }
        public string CodeMd { get; set; }
        public string TextMd { get; set; }
        public IEnumerable<string> CodeInappropriateWords { get; set; }
        public IEnumerable<string> TextInappropriateWords { get; set; }
    }
}
