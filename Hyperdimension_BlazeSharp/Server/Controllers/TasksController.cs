﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Hyperdimension_BlazeSharp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Hyperdimension_BlazeSharp.Server.Repositories;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IModuleRepository _moduleRepository;

        public TasksController(ITaskRepository taskRepository, IModuleRepository moduleRepository)
        {
            _taskRepository = taskRepository;
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            return Ok(await _taskRepository.GetTasks());
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
            var task = await _taskRepository.GetSpecyficTask(id);

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
            var module = await _moduleRepository.TryGetModuleById(taskCreateRequest.ModuleId);

            if(module is null)
            {
                return NotFound();
            }

            await _taskRepository.CreateTask(taskCreateRequest);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var task = await _taskRepository.TryGetTaskIfExist(id);

            if(task is null)
            {
                return BadRequest();
            }

            await _taskRepository.DeleteTask(task);

            return Ok();
        }

        [HttpGet("history/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<UserTaskHistory>>> GetUserTaskHistory(Guid userId)
        {
            var history = await _taskRepository.GetUserTaskHistory(userId);

            if(history is null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        [Authorize]
        [HttpPost("history/submittask")]
        public async Task<ActionResult<bool>> SubmitTask(SubmitTaskData submitTaskData)
        {        
            //var task = await _db.Tasks.Where(x => x.Id == submitTaskData.TaskId).FirstOrDefaultAsync();
            //var user = await _db.Users.Where(x => x.Email == HttpContext.User.FindFirst("Name").Value).Include(x => x.UsersDetails).FirstOrDefaultAsync();

            //if(task is null || user is null)
            //{
            //    return false;
            //}

            //var previousAttempt = await _db.UserTaskHistory.Where(x => x.UserId == user.Id && x.TaskId == task.Id).FirstOrDefaultAsync();

            //if (previousAttempt is null)
            //{
            //    user.UsersDetails.Points += (int)task.Points;

            //    UserTaskHistory userTaskHistory = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Solution = submitTaskData.Solution,
            //        IsTaskPassed = submitTaskData.IsTaskPassed,
            //        SubmittedAt = DateTime.Now,
            //        User = user,
            //        Task = task
            //    };

            //    await _db.UserTaskHistory.AddAsync(userTaskHistory);                
            //}
            //else
            //{                
            //    previousAttempt.Solution = submitTaskData.Solution;
            //    previousAttempt.SubmittedAt = DateTime.Now;
            //    previousAttempt.IsTaskPassed = submitTaskData.IsTaskPassed;
            //}

            //await _db.SaveChangesAsync();

            return true;
        }
    }
}
