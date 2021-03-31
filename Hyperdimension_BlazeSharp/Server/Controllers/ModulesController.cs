﻿using Hyperdimension_BlazeSharp.Shared.Models;
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
        public async Task<ActionResult<List<Modules>>> GetModulesWithTasks(int mode)
        {
            return await _db.Modules.Where(x => x.Mode == mode).Include("Tasks").ToListAsync();        
        }
    }
}
