using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("Folks")]
    [ApiController]
    public class FolksController : Controller
    {
        private readonly HblazesharpContext _db;

        public FolksController(HblazesharpContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FolkStory>>> GetFolks()
        {
            return await _db.FolkStories
                .Select(x => new FolkStory()
                {
                    ImgUrl = x.ImageUrl,
                    Story = x.Story,
                    Title = x.Title
                })
                .ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FolkStory>> GetFolk(Guid id)
        {
            var result = await _db.FolkStories
                .Where(x => x.Id == id)
                .Select(x => new FolkStory()
                {
                    ImgUrl = x.ImageUrl,
                    Story = x.Story,
                    Title = x.Title
                })
                .FirstOrDefaultAsync();

            if(result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
