using Hyperdimension_BlazeSharp.Server.Repositories;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("Comments")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{taskId:guid}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(Guid taskId)
        {
            return Ok(await _commentRepository.GetCommentsWithSubcomments(taskId));
        }
    }
}
