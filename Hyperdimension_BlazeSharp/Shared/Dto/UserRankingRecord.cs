using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record UserRankingRecord : UserMinimal
    {
        public Guid Guid { get; }

        public UserRankingRecord(string email, int points, Guid guid)
            : base(email, points) => Guid = guid;
    }
}
