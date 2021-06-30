using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class ForumViewModel : IForumViewModel
    {
        private readonly HttpClient _httpClient;

        public ForumViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid taskId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>($"/Comments/{taskId}");
        }
    }
}
