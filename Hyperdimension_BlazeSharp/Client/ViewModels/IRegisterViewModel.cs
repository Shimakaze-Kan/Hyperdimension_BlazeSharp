using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IRegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Task<HttpResponseMessage> RegisterUser();
    }
}
