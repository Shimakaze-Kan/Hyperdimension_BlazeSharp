using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Hyperdimension_BlazeSharp.Server.Models;

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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDataPlayground>> GetSpecyficTask(Guid id)
        {
            var task = await _db.Tasks.Where(x => x.Id == id)
                .Select(task => 
                    new TaskDataPlayground(task.Id, task.Title, task.Points, task.Description, task.InitialCode, task.TestCode))
                .FirstOrDefaultAsync();

            if(task is null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpGet("history/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<UserTaskHistory>>> GetUserTaskHistory(Guid userId)
        {
            var history = await _db.UserTaskHistory.Where(x => x.UserId == userId)
                                            .Where(x => x.IsTaskPassed == 1)
                                            .Include(x => x.Task)                                            
                                            .ToListAsync();

            if(history is null)
            {
                return NotFound();
            }

            return history;
        }
    }
}
