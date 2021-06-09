using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(HblazesharpContext hblazesharpContext) : base(hblazesharpContext)
        {
        }

        public async Task ChangeUserPreferences(UserPreferencesForce userPreferences, Users preferences)
        {            
            if (userPreferences.ThemeId is not null)
            {
                preferences.UserPreferencesId = (int)userPreferences.ThemeId;
            }

            preferences.UsersDetails.About = userPreferences.About;

            if (!string.IsNullOrEmpty(userPreferences.AvatarUrl))
            {
                preferences.UsersDetails.AvatarUrl = userPreferences.AvatarUrl;
            }

            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task ChangeUserPreferencesForce(UserPreferencesForce userPreferencesForce, Users preferences)
        {
            if (userPreferencesForce.ThemeId is not null)
            {
                preferences.UserPreferencesId = (int)userPreferencesForce.ThemeId;
            }

            preferences.UsersDetails.About = userPreferencesForce.About;

            if (!string.IsNullOrEmpty(userPreferencesForce.AvatarUrl))
            {
                preferences.UsersDetails.AvatarUrl = userPreferencesForce.AvatarUrl;
            }

            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRankingRecord>> GetRanking()
        {
            return await _hblazesharpContext.Users.Include(x => x.UsersDetails)
                .OrderByDescending(x => x.UsersDetails.Points)
                .Select(x => new UserRankingRecord(x.Email, x.UsersDetails.Points, x.Id))
                .ToListAsync();
        }

        public async Task<Users> GetPreferencesById(Guid id)
        {
            return await _hblazesharpContext.Users.Where(x => x.Id == id).Include(x => x.UsersDetails).SingleOrDefaultAsync();
        }

        public async Task<Users> GetUserByName(string name)
        {
            return await _hblazesharpContext.Users.Where(x => x.Email == name).Include(x => x.UsersDetails).SingleOrDefaultAsync();
        }

        public async Task<Users> GetUserPreferences(string name)
        {
            return await _hblazesharpContext.Users.Where(x => x.Email == name).Include(x => x.UsersDetails).SingleOrDefaultAsync();
        }

        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            return await _hblazesharpContext.Users.Where(user => user.Id == id)
                .Include(x => x.UsersDetails)
                .Include(x => x.UserTaskHistory)
                .ThenInclude(x => x.Task)
                .Select(user => new UserProfile(user.Email, user.UsersDetails.Points,
                    user.UsersDetails.AvatarUrl, user.UsersDetails.About,
                    user.UserTaskHistory
                        .Select(x => new TaskMinimalWithSubmissionDate(x.TaskId, x.Task.Title, x.SubmittedAt))))
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<UsersMinimal>> GetUsers()
        {
            return await _hblazesharpContext.Users.Include(x => x.UsersDetails)
                .Select(x => new UsersMinimal { Id = x.Id, Email = x.Email, Points = x.UsersDetails.Points }).ToListAsync();
        }

        public async Task<IEnumerable<FolkStory>> GetUsersFolkStories(Guid id)
        {
            return await _hblazesharpContext.Tasks.Where(x => x.Id == id)
                .Include(x => x.Module)
                .ThenInclude(x => x.FolkStory)
                .Select(x => new FolkStory
                {
                    ImgUrl = x.Module.FolkStory.ImageUrl,
                    Title = x.Module.FolkStory.Title,
                    StoryId = x.Module.FolkStory.Id
                })
                .ToListAsync();
        }

        public async Task<Users> RegisterUser(UserAuthRequest userAuthRequest)
        {
            UsersDetails usersDetails = new() { About = string.Empty };
            Users newAccount = new()
            {
                Id = Guid.NewGuid(),
                Email = userAuthRequest.Email,
                Password = PasswordHasher.Encrypt(userAuthRequest.Password),
                Role = "casual",
                Source = "sss",
                UsersDetails = usersDetails
            };

            _hblazesharpContext.Users.Add(newAccount);
            await _hblazesharpContext.SaveChangesAsync();

            return newAccount;
        }
    }
}
