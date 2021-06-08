using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface IModuleRepository
    {
        public Task<IEnumerable<ModuleItemMinimal>> GetAllModules();
        public Task<IEnumerable<ModuleWithTasks>> GetModuleWithTasks(int mode);
        public Task<Guid> CreateModuleWithFolkStory(CustomModuleCreateRequest customModuleCreateRequest);
        public Task DeleteModule(Models.Modules module);
        public Task<Models.Modules> TryGetModuleByTitle(string title);
        public Task<Models.Modules> TryGetModuleById(Guid id);
    }
}
