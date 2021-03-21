using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("staticimages")]
    [ApiController]
    public class StaticImagesController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetImages()
        {
            return new List<string>() { "https://i.imgur.com/CXNopVm.png", "http://japoland.pl/blog/wp-content/uploads/400px-Ikku_Mikosi-nyudo.jpg" };
        }
    }
}
