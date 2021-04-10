using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record UserAuthenticationMinimal : UserEmail
    {
        public string Password { get; init; }

        public UserAuthenticationMinimal(string email, string password)
            : base(email) => Password = password;
    }                
}
