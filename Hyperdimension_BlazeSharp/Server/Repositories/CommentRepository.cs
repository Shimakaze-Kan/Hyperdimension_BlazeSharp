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
                    Subcomments = x.Subcomments.Where(x => x.CommentId == x.Id)
                        .Select(x => new Subcomment()
                        {
                            AvatarUrl = x.User.UsersDetails.AvatarUrl,
                            UserName = x.User.Email,
                            MainCommentId = x.CommentId,
                            Date = x.SubmittedAt.ToString(),
                            Text = x.Text
                        })
                })
                .ToListAsync();
        }

        //public async Task<bool> CreateComment(Comment comment)
        //{
        //    await _hblazesharpContext.Comments.Add(new()
        //    {
        //        Id = Guid.NewGuid(),
        //        SubmittedAt = DateTime.Now,
        //        TaskId = comment.Id,
        //        UserId = comment.
        //    })
        //}
    }
}
