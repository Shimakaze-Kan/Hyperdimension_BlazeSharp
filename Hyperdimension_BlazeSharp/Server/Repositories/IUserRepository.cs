using Hyperdimension_BlazeSharp.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface IUserRepository
    {
        public Task<Users> GetUserByName(string name);
    }
}
