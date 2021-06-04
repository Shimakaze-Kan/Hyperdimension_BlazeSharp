using Hyperdimension_BlazeSharp.Server.Models;
using Hyperdimension_BlazeSharp.Shared.Dto;
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
        private readonly HblazesharpContext _db;

        public ModulesController(HblazesharpContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Modules>>> GetModules()
        {
            return (await _db.Modules.ToListAsync()).OrderBy(x => x.Id).ToList();
        }

        [HttpGet("mode/{mode:int}")]
        public async Task<ActionResult<IEnumerable<ModuleWithTasks>>> GetModulesWithTasks(int mode)
        {
            return await _db.Modules.Where(x => x.Mode == mode)
                .Include(m => m.Tasks)
                .Select(module => 
                    new ModuleWithTasks(module.Title, module.Tasks
                    .Select(task => 
                        new TaskMinimalWithPoints(task.Id, task.Title, task.Points)))).ToListAsync();       
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateModule(CustomModuleCreateRequest customModuleCreateRequest)
        {
            var module = await _db.Modules.SingleOrDefaultAsync(x => x.Title == customModuleCreateRequest.Title);

            if(module is not null)
            {
                return BadRequest();
            }

            module = new();
            module.Id = Guid.NewGuid();
            module.Title = customModuleCreateRequest.Title;
            module.Mode = customModuleCreateRequest.Mode;

            await _db.Modules.AddAsync(module);                        

            await _db.SaveChangesAsync();

            return Ok(module.Id);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteModule(Guid id)
        {
            var module = await _db.Modules.SingleOrDefaultAsync(x => x.Id == id);

            if(module is null)
            {
                return NotFound();
            }

            _db.Modules.Remove(module);
            await _db.SaveChangesAsync();
            
            return Ok();
        }
    }
}
