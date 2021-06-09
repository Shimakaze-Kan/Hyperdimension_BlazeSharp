using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Server.Repositories;
using Hyperdimension_BlazeSharp.Shared;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public UsersController(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersMinimal>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsers());
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<IEnumerable<UserRankingRecord>>> GetRanking()
        {
            return Ok(await _userRepository.GetRanking());
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
        {
            var user = await _userRepository.GetUserProfile(id);

            if (user is null)
            {
                return NotFound();
            }

            if (!user.Tasks.Any())
                return user;

            var folk = await _userRepository.GetUsersFolkStories(id);

            return user with { AchievedStories = folk };
        }

        [HttpPost("loginuser")]
        public async Task<ActionResult<UserAuthResult>> LoginUser([FromForm] UserAuthRequest userAuthRequest)
        {
            var user = await _userRepository.GetUserByName(userAuthRequest.Email);

            if (user is null)
            {
                return BadRequest("Incorrect email");
            }

            if (PasswordHasher.Verify(userAuthRequest.Password, user.Password) is false)
            {
                return BadRequest("Incorrect password");
            }

            return new UserAuthResult() 
            { 
                Email = user.Email, 
                Token = _jwtTokenService.BuildToken(user.Email, user.Id, user.Role)
            };
        }

        [HttpPost("registeruser")]
        public async Task<ActionResult<UserAuthResult>> RegisterUser(UserAuthRequest userAuthenticationMinimal)
        {
            var user = await _userRepository.GetUserByName(userAuthenticationMinimal.Email);

            if (user is not null)
            {
                return BadRequest("There is already a user with this email");
            }

            var newAccount = await _userRepository.RegisterUser(userAuthenticationMinimal);

            return new UserAuthResult()
            {
                Email = newAccount.Email,
                Token = _jwtTokenService.BuildToken(newAccount.Email, newAccount.Id, newAccount.Role)
            };
        }

        [Authorize]
        [HttpPost("changeuserpreferences")]
        public async Task<IActionResult> ChangeUserPreferences(UserPreferencesForce userPreferences)
        {
            var preferences = await _userRepository.GetUserPreferences(HttpContext.User.FindFirstValue("name"));

            if(preferences is null)
            {
                return BadRequest();
            }

            await _userRepository.ChangeUserPreferences(userPreferences, preferences);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("changeUserPreferencesForce")]
        public async Task<IActionResult> ChangeUserPreferencesForce(UserPreferencesForce userPreferencesForce)
        {
            var preferences = await _userRepository.GetPreferencesById(userPreferencesForce.UserId);

            if(preferences is null)
            {
                return BadRequest();
            }

            await _userRepository.ChangeUserPreferencesForce(userPreferencesForce, preferences);
            return Ok();
        }
    }
}
