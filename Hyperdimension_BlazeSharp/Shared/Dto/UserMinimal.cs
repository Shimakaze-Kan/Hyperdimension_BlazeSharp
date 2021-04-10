using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record UserMinimal : UserEmail
    {
        public int Points { get; init; }

        public UserMinimal(string email, int points)
            : base(email) => Points = points;
    }
}
