using Hyperdimension_BlazeSharp.Shared.Models;
using Hyperdimension_BlazeSharp.Shared;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : Controller
    {
        List<Tuple<string,int>> list = new() { new("user1", 12), new("user1212", 122), new("user1000", 4) };
        private readonly HblazesharpContext _db;

        public UsersController(HblazesharpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsersMinimal>>> GetUsers()
        {
            return await _db.Users.Include(x => x.UsersDetails)
                .Select(x => new UsersMinimal { Id = x.Id, Email = x.Email, Points = x.UsersDetails.Points }).ToListAsync();
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<IEnumerable<UserRankingRecord>>> GetRanking()
        {
            return (await _db.Users.Include(x => x.UsersDetails)
                .Select(x => new UserRankingRecord(x.Email, x.UsersDetails.Points, x.Id)).ToListAsync())
                .OrderByDescending(m => m.Points).ToList();
        }

        [HttpGet("test")]
        public async Task<ActionResult<Tuple<string,string>>> GetName()
        {
            var oldDate = await _db.Users.Where(x => x.Email == "Test@tesat.test122").FirstOrDefaultAsync();

            if(oldDate is not null)
            {
                _db.Users.Remove(oldDate);
            }

            UsersDetails usersDetails = new() { About = "hi" };
            Users user = new()
            {
                Id = Guid.NewGuid(),
                Email = "Test@tesat.test122",
                Password = "123",
                Role = "casual",
                Source = "qqq",
                UsersDetails = usersDetails
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var id = user.Id;

            var ret = await _db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            return new Tuple<string, string>(ret.Id.ToString(), ret.Email);
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
        {
            var user = await _db.Users.Where(user => user.Id == id)
                .Include(x => x.UsersDetails)
                .Include(x => x.UserTaskHistory)
                .ThenInclude(x => x.Task)
                .Select(user => new UserProfile(user.Email, user.UsersDetails.Points, 
                    user.UsersDetails.AvatarUrl, user.UsersDetails.About, 
                    user.UserTaskHistory
                        .Select(x => new TaskMinimalWithSubmissionDate(x.TaskId, x.Task.Title, x.SubmittedAt))))
                .FirstOrDefaultAsync();

            if(user is null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
