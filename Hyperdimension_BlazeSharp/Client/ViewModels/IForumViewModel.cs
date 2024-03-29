﻿using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IForumViewModel
    {
        CommentCreateRequest CommentCreateRequest { get; set; }
        SubcommentCreateRequest SubcommentCreateRequest { get; set; }
        ProfanityScannerResponse ProfanityScannerResponse { get; set; }
        IEnumerable<Comment> Comments { get; set; }
        Guid TaskId { get; set; }
        public ValueWrapper<bool> SendInappropriate { get; set; }

        Task CreateNewComment();
        Task CreateNewSubcomment();
        Task GetComments();
        Task CheckComment();
        Task CheckSubcomment();
    }
}