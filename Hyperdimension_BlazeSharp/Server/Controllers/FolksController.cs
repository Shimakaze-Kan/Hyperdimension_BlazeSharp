using Hyperdimension_BlazeSharp.Server.Repositories;
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
        private readonly IFolkRepository _folkRepository;

        public FolksController(IFolkRepository folkRepository)
        {
            _folkRepository = folkRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FolkStory>>> GetFolks()
        {
            return Ok(await _folkRepository.GetAllFolkStories());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FolkStory>> GetFolk(Guid id)
        {
            var result = await _folkRepository.GetSpecyficFolkStory(id);

            if(result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
