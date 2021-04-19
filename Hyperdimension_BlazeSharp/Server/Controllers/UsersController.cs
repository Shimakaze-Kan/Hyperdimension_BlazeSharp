using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : Controller
    {
        List<Tuple<string, int>> list = new() { new("user1", 12), new("user1212", 122), new("user1000", 4) };
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
        public async Task<ActionResult<Tuple<string, string>>> GetName()
        {
            var oldDate = await _db.Users.Where(x => x.Email == "Test@tesat.test122").FirstOrDefaultAsync();

            if (oldDate is not null)
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

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("loginuser")]
        public async Task<IActionResult> LoginUser(UserAuthenticationMinimal userAuthenticationMinimal)
        {
            var user = await _db.Users.Where(x => x.Email == userAuthenticationMinimal.Email)
                .Select(x => new UserAuthenticationMinimal(x.Email, x.Password))
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return BadRequest("Incorrect email");
            }

            if (PasswordHasher.Verify(userAuthenticationMinimal.Password, user.Password) is false)
            {
                return BadRequest("Incorrect password");
            }


            var claims = new Claim(ClaimTypes.Name, user.Email);
            var claimsIdentity = new ClaimsIdentity(new[] { claims }, "ServerSideAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);


            return Ok();
        }

        [HttpPost("registeruser")]
        public async Task<ActionResult<UserAuthenticationMinimal>> RegisterUser(UserAuthenticationMinimal userAuthenticationMinimal)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == userAuthenticationMinimal.Email);

            if (user is not null)
            {
                return BadRequest("User exists"); //tmp solution
            }

            Users newAccount = new()
            {
                Id = Guid.NewGuid(),
                Email = userAuthenticationMinimal.Email,
                Password = PasswordHasher.Encrypt(userAuthenticationMinimal.Password),
                Role = "casual",
                Source = "sss"
            };

            _db.Users.Add(newAccount);
            await _db.SaveChangesAsync();

            return await Task.FromResult(userAuthenticationMinimal);
        }

        [HttpGet("logoutuser")]
        public async Task<ActionResult<string>> LogoutUser()
        {
            await HttpContext.SignOutAsync();
            return "done";
        }

        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<UserGuidEmail>> GetCurrentuser()
        {
            UserGuidEmail userGuidEmail = new(default, null);

            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Name);
                userGuidEmail = await _db.Users.Where(x => x.Email == userEmail).Select(x => new UserGuidEmail(x.Id, x.Email)).FirstOrDefaultAsync();
            }

            return await Task.FromResult(userGuidEmail);
        }
    }
}
