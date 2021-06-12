using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Server.Repositories;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("modules")]
    [ApiController]
    public class ModulesController : Controller
    {
        private readonly IModuleRepository _moduleRepository;

        public ModulesController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleItemMinimal>>> GetModules()
        {
            return Ok(await _moduleRepository.GetAllModules());
        }

        [HttpGet("mode/{mode:int}")]
        public async Task<ActionResult<IEnumerable<ModuleWithTasks>>> GetModulesWithTasks(int mode)
        {
            return Ok(await _moduleRepository.GetModuleWithTasks(mode)); 
        }

        [HttpPost]
        [Authorize(Roles = "admin, modulemode")]
        public async Task<ActionResult<Guid>> CreateModule(CustomModuleCreateRequest customModuleCreateRequest)
        {
            var module = await _moduleRepository.TryGetModuleByTitle(customModuleCreateRequest.Title);

            if(module is not null)
            {
                return BadRequest();
            }            

            return Ok(_moduleRepository.CreateModuleWithFolkStory(customModuleCreateRequest));
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "admin, modulemode")]
        public async Task<ActionResult> DeleteModule(Guid id)
        {
            var module = await _moduleRepository.TryGetModuleById(id);

            if(module is null)
            {
                return NotFound();
            }

            await _moduleRepository.DeleteModule(module);
            
            return Ok();
        }
    }
}
