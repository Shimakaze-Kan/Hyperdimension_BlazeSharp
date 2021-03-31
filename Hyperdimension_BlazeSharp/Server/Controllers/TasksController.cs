using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly HblazesharpContext _db;

        public TasksController(HblazesharpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tasks>>> GetTasks()
        {
            return await _db.Tasks.ToListAsync();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Tasks>> GetSpecyficTask(Guid id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if(task is null)
            {
                return NotFound();
            }

            return task;
        }
    }
}
