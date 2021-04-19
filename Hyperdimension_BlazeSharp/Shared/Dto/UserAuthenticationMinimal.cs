using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class UserAuthenticationMinimal
    {
        [Required, MaxLength(255), EmailAddress]
        public string Password { get; init; }
        [Required, MaxLength(255), MinLength(5)]
        public string Email { get; init; }

        public UserAuthenticationMinimal(string email, string password)
            => (Email, Password) = (email, password);
    }                
}
