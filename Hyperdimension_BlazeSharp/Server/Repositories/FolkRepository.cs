using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public class FolkRepository : BaseRepository, IFolkRepository
    {
        public FolkRepository(HblazesharpContext hblazesharpContext) : base(hblazesharpContext)
        {
        }

        public async Task<IEnumerable<FolkStory>> GetAllFolkStories()
        {
            return await _hblazesharpContext.FolkStories
                .Select(x => new FolkStory()
                {
                    ImgUrl = x.ImageUrl,
                    Story = x.Story,
                    Title = x.Title
                })
                .ToListAsync();
        }

        public async Task<FolkStory> GetSpecyficFolkStory(Guid id)
        {
            return await _hblazesharpContext.FolkStories
                .Where(x => x.Id == id)
                .Select(x => new FolkStory()
                {
                    ImgUrl = x.ImageUrl,
                    Story = x.Story,
                    Title = x.Title
                })
                .SingleOrDefaultAsync();
        }
    }
}
