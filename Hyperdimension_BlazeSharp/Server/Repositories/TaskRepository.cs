using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(HblazesharpContext hblazesharpContext) : base(hblazesharpContext)
        {
        }

        public async Task CreateTask(TaskCreateRequest taskCreateRequest)
        {
            await _hblazesharpContext.Tasks.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Title = taskCreateRequest.Title,
                Description = taskCreateRequest.Description,
                InitialCode = taskCreateRequest.InitialCode,
                ModuleId = taskCreateRequest.ModuleId,
                TestCode = taskCreateRequest.TestCode,
                Points = taskCreateRequest.Points
            });

            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task DeleteTask(Tasks task)
        {
            _hblazesharpContext.Tasks.Remove(task);
            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task<TaskDataPlayground> GetSpecyficTask(Guid id)
        {
            return await _hblazesharpContext.Tasks.Where(x => x.Id == id).Include(x => x.Module)
                .Select(task =>
                    new TaskDataPlayground(task.Id, task.Title, task.Points, task.Description, task.InitialCode, task.TestCode, task.Module.Mode))
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasks()
        {
            return await _hblazesharpContext.Tasks.ToListAsync();
        }

        public async Task<bool> CheckIfSpecyficTaskPassed(Guid userId, Guid taskId)
        {
            var result = await _hblazesharpContext.UserTaskHistory.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == taskId);
            
            if(result is not null)
            {
                return result.IsTaskPassed == 1;
            }

            return false;
        }

        public async Task<IEnumerable<UserTaskHistory>> GetUserTaskHistory(Guid userId)
        {
            return await _hblazesharpContext.UserTaskHistory.Where(x => x.UserId == userId)
                                            .Where(x => x.IsTaskPassed == 1)
                                            .Include(x => x.Task)
                                            .ToListAsync();
        }

        public async Task SubmitTask(SubmitTaskData submitTaskData, Users user, Tasks task)
        {
            var previousAttempt = await _hblazesharpContext.UserTaskHistory.SingleOrDefaultAsync(x => x.UserId == user.Id && x.TaskId == task.Id);

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

                await _hblazesharpContext.UserTaskHistory.AddAsync(userTaskHistory);
            }
            else
            {
                previousAttempt.Solution = submitTaskData.Solution;
                previousAttempt.SubmittedAt = DateTime.Now;
                previousAttempt.IsTaskPassed = submitTaskData.IsTaskPassed;
            }

            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task<Tasks> TryGetTaskIfExist(Guid id)
        {
            return await _hblazesharpContext.Tasks.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
