using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IForumViewModel
    {
        Task<IEnumerable<Comment>> GetComments(Guid taskId);
    }
}