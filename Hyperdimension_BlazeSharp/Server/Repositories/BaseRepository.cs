using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly HblazesharpContext _hblazesharpContext;

        public BaseRepository(HblazesharpContext hblazesharpContext)
        {
            _hblazesharpContext = hblazesharpContext;
        }
    }
}
