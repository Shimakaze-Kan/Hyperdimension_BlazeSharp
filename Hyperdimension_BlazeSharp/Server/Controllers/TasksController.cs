using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Hyperdimension_BlazeSharp.Server.Models;
using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Receives task by id
        /// </summary>
        /// 
        /// <param name="id">Task id</param>
        /// <returns>TaskDataPlayground object</returns>
        /// <response code="401">If task doesn't exist</response>
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

        [HttpPost("history/submittask")]
        public async Task<ActionResult<bool>> SubmitTask(SubmitTaskData submitTaskData)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }            

            var task = await _db.Tasks.Where(x => x.Id == submitTaskData.TaskId).FirstOrDefaultAsync();
            var user = await _db.Users.Where(x => x.Email == User.Identity.Name).Include(x => x.UsersDetails).FirstOrDefaultAsync();

            if(task is null || user is null)
            {
                return false;
            }

            var previousAttempt = await _db.UserTaskHistory.Where(x => x.UserId == user.Id && x.TaskId == task.Id).FirstOrDefaultAsync();

            if (previousAttempt is null)
            {
                user.UsersDetails.Points += (int)task.Points;

                UserTaskHistory userTaskHistory = new()
                {
                    Id = Guid.NewGuid(),
                    Solution = submitTaskData.Solution,
                    IsTaskPassed = submitTaskData.IsTaskPassed,
                    SubmittedAt = DateTime.Now,
                    User = user,
                    Task = task
                };

                await _db.UserTaskHistory.AddAsync(userTaskHistory);                
            }
            else
            {                
                previousAttempt.Solution = submitTaskData.Solution;
                previousAttempt.SubmittedAt = DateTime.Now;
                previousAttempt.IsTaskPassed = submitTaskData.IsTaskPassed;
            }

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
