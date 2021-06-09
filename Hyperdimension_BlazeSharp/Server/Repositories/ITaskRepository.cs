using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<Tasks>> GetTasks();
        public Task<TaskDataPlayground> GetSpecyficTask(Guid id);
        public Task CreateTask(TaskCreateRequest taskCreateRequest);
        public Task DeleteTask(Tasks task);
        public Task<IEnumerable<UserTaskHistory>> GetUserTaskHistory(Guid userId);
        public Task SubmitTask(SubmitTaskData submitTaskData, Users user, Tasks task);
        public Task<Tasks> TryGetTaskIfExist(Guid id);
    }
}
