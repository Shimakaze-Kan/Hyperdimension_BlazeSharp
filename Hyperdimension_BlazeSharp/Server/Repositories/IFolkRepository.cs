using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface IFolkRepository
    {
        public Task<FolkStory> GetSpecyficFolkStory(Guid id);
        public Task<IEnumerable<FolkStory>> GetAllFolkStories();

    }
}
