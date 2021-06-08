using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public class ModuleRepository : BaseRepository, IModuleRepository
    {
        public ModuleRepository(HblazesharpContext hblazesharpContext) : base(hblazesharpContext)
        {
        }

        public async Task<Guid> CreateModuleWithFolkStory(CustomModuleCreateRequest customModuleCreateRequest)
        {
            Models.Modules module = new()
            {
                Id = Guid.NewGuid(),
                Title = customModuleCreateRequest.Title,
                Mode = customModuleCreateRequest.Mode
            };

            if (customModuleCreateRequest.IsFolkStory)
            {
                module.FolkStory = new()
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = customModuleCreateRequest.FolkStoryImageUrl,
                    Story = customModuleCreateRequest.FolkStoryStory,
                    Title = customModuleCreateRequest.Title
                };
            }

            await _hblazesharpContext.Modules.AddAsync(module);

            await _hblazesharpContext.SaveChangesAsync();

            return module.Id;
        }

        public async Task DeleteModule(Models.Modules module)
        {
            _hblazesharpContext.Modules.Remove(module);
            await _hblazesharpContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModuleItemMinimal>> GetAllModules()
        {
            return await _hblazesharpContext.Modules.OrderBy(x => x.Id)
                .Select(x => new ModuleItemMinimal() { Id = x.Id, Title = x.Title, Mode = x.Mode })
                .ToListAsync();
        }

        public async Task<IEnumerable<ModuleWithTasks>> GetModuleWithTasks(int mode)
        {
            return await _hblazesharpContext.Modules.Where(x => x.Mode == mode)
                .Include(m => m.Tasks)
                .Select(module =>
                    new ModuleWithTasks(module.Title, module.Tasks
                    .Select(task =>
                        new TaskMinimalWithPoints(task.Id, task.Title, task.Points)))).ToListAsync();
        }

        public async Task<Modules> TryGetModuleById(Guid id)
        {
            return await _hblazesharpContext.Modules.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Modules> TryGetModuleByTitle(string title)
        {
            return await _hblazesharpContext.Modules.SingleOrDefaultAsync(x => x.Title == title);
        }
    }
}
