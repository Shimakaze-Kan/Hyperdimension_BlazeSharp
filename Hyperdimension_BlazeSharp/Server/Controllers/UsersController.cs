using Hyperdimension_BlazeSharp.Server.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<Tuple<string, int>>>> GetUsers()
        {
            await System.Threading.Tasks.Task.Delay(1000);

            return list;
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<List<Tuple<string, int>>>> GetRanking()
        {
            await System.Threading.Tasks.Task.Delay(1000);

            return list.OrderByDescending(x => x.Item2).ToList();
        }
    }
}
