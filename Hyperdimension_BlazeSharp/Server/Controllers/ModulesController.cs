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

        [HttpGet("list")]
        public async Task<ActionResult<List<ModuleWithTasks>>> GetModulesWithTasks()
        {
            await System.Threading.Tasks.Task.Delay(1000);

            return new List<ModuleWithTasks>
            {
                new ModuleWithTasks()
                {
                    Module = new()
                    {
                        Id = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
                        FolkStoryId = "a4b7c3e4-ad26-44fc-b8c0-e25ac7da074c",
                        Title = "Tutorial",
                        Mode = 1
                    },
                    Tasks = new List<Shared.Task>()
                    {
                        new()
                        {
                            Id = "853afe7a-349e-4354-b4a2-6f27d40af987",
                            ModuleId = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam nec pharetra dui",
                            Title = "Task nr 1",
                            InitialCode = @"using System;
namespace BlazeSharpPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}",
                            TestCode = "",
                            Points = 1
                        },

                        new()
                        {
                            Id = "9e4ef438-bb2f-44e7-bf27-07103c2c31cf",
                            ModuleId = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam nec pharetra dui",
                            Title = "Task nr 2",
                            InitialCode = @"using System;
namespace BlazeSharpPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}",
                            TestCode = "Hell World!",
                            Points = 1
                        }
                    }

                }
            };
        }
    }
}
