using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared;
using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface IUserRepository
    {
        public Task<Users> GetUserByName(string name);
        public Task<IEnumerable<UsersMinimal>> GetUsers();
        public Task<IEnumerable<UserRankingRecord>> GetRanking();
        public Task<UserProfile> GetUserProfile(Guid id);
        public Task<Users> RegisterUser(UserAuthRequest userAuthRequest);
        public Task ChangeUserPreferences(UserPreferencesForce userPreferences, Users preferences);
        public Task ChangeUserPreferencesForce(UserPreferencesForce userPreferencesForce, Users preferences);
        public Task<Users> GetUserPreferences(string name);
        public Task<IEnumerable<FolkStory>> GetUsersFolkStories(Guid id);
        public Task<Users> GetPreferencesById(Guid id);
    }
}
