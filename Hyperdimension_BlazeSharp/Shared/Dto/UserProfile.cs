using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public record UserProfile : UserMinimal
    {
        public string AvatarUrl { get; init; }
        public string About { get; init; }
        public IEnumerable<TaskMinimalWithSubmissionDate> Tasks { get; init; }

        public UserProfile(string email, int points, string avatarUrl, string about, IEnumerable<TaskMinimalWithSubmissionDate> tasks)
            : base(email, points) => (AvatarUrl, About, Tasks) = (avatarUrl, about, tasks);
    }
}
