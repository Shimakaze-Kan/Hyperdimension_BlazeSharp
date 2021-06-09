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
using Hyperdimension_BlazeSharp.Server.Repositories;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IUserRepository _userRepository;

        public TasksController(ITaskRepository taskRepository,
            IModuleRepository moduleRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _moduleRepository = moduleRepository;
            _userRepository = userRepository;
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
            var task = await _taskRepository.TryGetTaskIfExist(submitTaskData.TaskId);
            var user = await _userRepository.GetUserByName(HttpContext.User.FindFirst("Name").Value);

            if (task is null || user is null)
            {
                return false;
            }

            await _taskRepository.SubmitTask(submitTaskData, user);

            return true;
        }
    }
}
