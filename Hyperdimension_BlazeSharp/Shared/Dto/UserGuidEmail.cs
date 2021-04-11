using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record UserGuidEmail : UserEmail
    {
        public Guid Guid { get; set; }

        public UserGuidEmail(Guid guid, string email)
            : base(email) => Guid = guid;
    }
}
