﻿using Hyperdimension_BlazeSharp.Server.Repositories;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Controllers
{
    [Route("Comments")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        [HttpGet("{taskId:guid}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(Guid taskId)
        {
            return Ok(await _commentRepository.GetCommentsWithSubcomments(taskId));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateComment(CommentCreateRequest commentCreateRequest)
        {
            var user = await _userRepository.GetUserByName(HttpContext.User.FindFirstValue("name"));
            var result = await _commentRepository.CreateComment(commentCreateRequest, user.Id);

            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("Subcomments")]
        public async Task<ActionResult> CreateSubcomment(SubcommentCreateRequest subcommentCreateRequest)
        {
            var user = await _userRepository.GetUserByName(HttpContext.User.FindFirstValue("name"));
            var result = await _commentRepository.CreateSubcomment(subcommentCreateRequest, user.Id);

            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpGet("CheckComment")]
        public async Task<ActionResult<ProfanityScannerResponse>> CheckComment([FromQuery]CommentCreateRequest commentCreateRequest)
        {
            var user = await _userRepository.GetUserByName(HttpContext.User.FindFirstValue("name"));
            var result = await _commentRepository.CheckProfanity(commentCreateRequest, user.Id);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("CheckSubcomment")]
        public async Task<ActionResult<ProfanityScannerResponse>> CheckSubcomment([FromQuery]SubcommentCreateRequest subcommentCreateRequest)
        {
            var result = await _commentRepository.CheckProfanity(subcommentCreateRequest);

            return Ok(result);
        }
    }
}
