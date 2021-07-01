using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(HblazesharpContext hblazesharpContext) : base(hblazesharpContext)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsWithSubcomments(Guid taskId)
        {
            return await _hblazesharpContext.Comments.Where(x => x.TaskId == taskId)
                .Include(x => x.Subcomments)
                .ThenInclude(x => x.User)
                .Select(x => new Comment()
                {
                    Id = x.Id,
                    AvatarUrl = x.User.UsersDetails.AvatarUrl,
                    Date = x.SubmittedAt.ToString(),
                    Text = x.Text,
                    UserName = x.User.Email,
                    Subcomments = x.Subcomments.Where(x => x.CommentId == x.Comment.Id)
                        .Select(x => new Subcomment()
                        {
                            AvatarUrl = x.User.UsersDetails.AvatarUrl,
                            UserName = x.User.Email,
                            MainCommentId = x.CommentId,
                            Date = x.SubmittedAt.ToString(),
                            Text = x.Text
                        })
                        .OrderBy(x => x.Date)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        public async Task<bool> CreateComment(CommentCreateRequest commentCreateRequest, Guid userId)
        {
            var prevCode = string.Empty;

            if(commentCreateRequest.AddLastSubmittedVersion)
            {
                var historyCode = await _hblazesharpContext.UserTaskHistory.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == commentCreateRequest.TaskId);
                prevCode = $"```{Environment.NewLine}{historyCode.Solution}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}";
            }

            var result = await _hblazesharpContext.Comments.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                SubmittedAt = DateTime.Now,
                TaskId = commentCreateRequest.TaskId,
                UserId = userId,
                Text = prevCode + commentCreateRequest.Text
            });

            if(result is null)
            {
                return false;
            }

            await _hblazesharpContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateSubcomment(SubcommentCreateRequest subcommentCreateRequest, Guid userId)
        {
            var result = await _hblazesharpContext.Subcomments.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CommentId = subcommentCreateRequest.MainCommentId,
                Text = subcommentCreateRequest.Text,
                SubmittedAt = DateTime.Now
            });

            if(result is null)
            {
                return false;
            }

            await _hblazesharpContext.SaveChangesAsync();

            return true;
        }
    }
}
