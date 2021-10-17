using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsWithSubcomments(Guid taskId);
        Task<bool> CreateComment(CommentCreateRequest commentCreateRequest, Guid userId);
        Task<bool> CreateSubcomment(SubcommentCreateRequest subcommentCreateRequest, Guid userId);
        Task<ProfanityScannerResponse> CheckProfanity(CommentCreateRequest commentCreateRequest, Guid userId);
        Task<ProfanityScannerResponse> CheckProfanity(SubcommentCreateRequest subcommentCreateRequest);
    }
}