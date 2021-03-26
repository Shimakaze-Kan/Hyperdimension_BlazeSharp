using Hyperdimension_BlazeSharp.Shared;
using Microsoft.AspNetCore.Mvc;
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
        public string TmpData { get; set; } = "294e75fa-1f3a-4d96-8cef-5a44aa29364a"; //testing purposes

        [HttpGet]
        public async Task<ActionResult<List<Module>>> GetModules()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            return new List<Module>() { new () { Id = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
            FolkStoryId = "a4b7c3e4-ad26-44fc-b8c0-e25ac7da074c",
            Title = "Tutorial",
            Mode = 1},
            new () { Id = "a45372a5-2139-4a68-a308-6129580f628b",
            FolkStoryId = "e2b3b398-d433-469c-bcf9-bc57e4a7c6f9",
            Title = "Tutorial",
            Mode = 1}};
        }
        
    }
}
