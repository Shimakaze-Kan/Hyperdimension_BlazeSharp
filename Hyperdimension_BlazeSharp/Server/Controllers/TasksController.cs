using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Hyperdimension_BlazeSharp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateTask(TaskCreateRequest taskCreateRequest)
        {
            var module = await _db.Modules.SingleOrDefaultAsync(x => x.Id == taskCreateRequest.ModuleId);

            if(module is null)
            {
                return NotFound();
            }

            await _db.Tasks.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Title = taskCreateRequest.Title,
                Description = taskCreateRequest.Description,
                InitialCode = taskCreateRequest.InitialCode,
                ModuleId = taskCreateRequest.ModuleId,
                TestCode = taskCreateRequest.TestCode,
                Points = taskCreateRequest.Points
            });

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var task = await _db.Tasks.SingleOrDefaultAsync(x => x.Id == id);

            if(task is null)
            {
                return BadRequest();
            }

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            return Ok();
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

        [Authorize]
        [HttpPost("history/submittask")]
        public async Task<ActionResult<bool>> SubmitTask(SubmitTaskData submitTaskData)
        {
            //if(!User.Identity.IsAuthenticated)
            //{
            //    return BadRequest();
            //}            

            var task = await _db.Tasks.Where(x => x.Id == submitTaskData.TaskId).FirstOrDefaultAsync();
            var user = await _db.Users.Where(x => x.Email == HttpContext.User.FindFirst("Name").Value).Include(x => x.UsersDetails).FirstOrDefaultAsync();

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
