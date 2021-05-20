using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class UserAuthResult
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public Guid Guid { get; set; }
    }
}
