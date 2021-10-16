using Hyperdimension_BlazeSharp.Server.Service;
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
        private readonly IProfanityScannerService _profanityScannerService;

        public CommentRepository(HblazesharpContext hblazesharpContext, IProfanityScannerService profanityScannerService) 
            : base(hblazesharpContext)
        {
            _profanityScannerService = profanityScannerService;
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
                prevCode = await GetLastSubmittedSolution(commentCreateRequest, userId);
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

        private async Task<string> GetLastSubmittedSolution(CommentCreateRequest commentCreateRequest, Guid userId)
        {
            var historyCode = await _hblazesharpContext.UserTaskHistory.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == commentCreateRequest.TaskId);
            return $"```{Environment.NewLine}{historyCode.Solution}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}";
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

        public async Task<ProfanityScannerResponse> CheckProfanity(CommentCreateRequest commentCreateRequest, Guid userId)
        {
            var prevCode = await GetLastSubmittedSolution(commentCreateRequest, userId);

            var code = await _profanityScannerService.FindProfanityInText(prevCode);
            var text = await _profanityScannerService.FindProfanityInText(commentCreateRequest.Text);

            var codeMd = prevCode;
            var textMd = commentCreateRequest.Text;

            foreach (var word in code)
            {
                codeMd = codeMd.Replace(word.Word, @$"<span style=""color: red"">{word.Word}</span>");
            }

            foreach (var word in text)
            {
                textMd = textMd.Replace(word.Word, @$"<span style=""color: red"">{word.Word}</span>");
            }

            return new ProfanityScannerResponse()
            {
                IsInappropriate = code.Count() > 0 || text.Count() > 0,
                CodeMd = codeMd,
                TextMd = textMd,
                CodeInappropriateWords = code.Select(x => x.Word),
                TextInappropriateWords = text.Select(x => x.Word)
            };
        }
    }
}
