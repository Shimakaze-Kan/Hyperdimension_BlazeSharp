using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server
{
    public interface IJwtTokenService
    {
        string BuildToken(string email, Guid guid, string role);
    }
}
