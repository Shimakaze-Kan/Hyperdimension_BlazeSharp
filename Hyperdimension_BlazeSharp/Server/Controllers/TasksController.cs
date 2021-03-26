using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Shared.Task>>> GetTasks()
        {
            await Task.Delay(1000);

            return new List<Shared.Task>() { new() { Id = "853afe7a-349e-4354-b4a2-6f27d40af987",
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
            Points = 1},

            new() { Id = "9e4ef438-bb2f-44e7-bf27-07103c2c31cf",
            ModuleId = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
            Description = "Make the program output Hell World! instead of Hello World!",
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
            Points = 1}};
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shared.Task>> GetSpecyficTask(string id)
        {
            await Task.Delay(1000);

            if (id == "853afe7a-349e-4354-b4a2-6f27d40af987")
            {
                return new Shared.Task()
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
                };
            }

            if (id == "9e4ef438-bb2f-44e7-bf27-07103c2c31cf")
            {
                return new Shared.Task()
                {
                    Id = "9e4ef438-bb2f-44e7-bf27-07103c2c31cf",
                    ModuleId = "e7d97879-a1cd-47e0-ace2-2b6911e9bc89",
                    Description = "Make the program output Hell World! instead of Hello World!",
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
                };
            }


            return NotFound();
        }
    }
}
