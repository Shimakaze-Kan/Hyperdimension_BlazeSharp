using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
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
        public CommentCreateRequest CommentCreateRequest { get; set; } = new();
        public SubcommentCreateRequest SubcommentCreateRequest { get; set; } = new();
        public ProfanityScannerResponse ProfanityScannerResponse { get; set; } = new();
        public IEnumerable<Comment> Comments { get; set; }
        public Guid TaskId { get; set; }

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ForumViewModel(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task GetComments()
        {
            Comments = await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>($"/Comments/{TaskId}");
        }

        public async Task CreateNewComment()
        {
            CommentCreateRequest.TaskId = TaskId;
            var result = await _httpClient.PostAsJsonAsyncJwtHeader(_localStorageService, CommentCreateRequest, "/Comments");

            if (result.IsSuccessStatusCode)
            {
                await GetComments();
            }

            CommentCreateRequest = new();
        }

        public async Task CreateNewSubcomment()
        {
            var result = await _httpClient.PostAsJsonAsyncJwtHeader(_localStorageService, SubcommentCreateRequest, "/Comments/Subcomments");

            if (result.IsSuccessStatusCode)
            {
                await GetComments();
            }

            SubcommentCreateRequest = new();
        }

        public async Task CheckComment()
        {
            CommentCreateRequest.TaskId = TaskId;
            ProfanityScannerResponse = await _httpClient.GetJsonAsyncJwtHeader<ProfanityScannerResponse>
                (_localStorageService, "/Comments/checkcomment?" + CommentCreateRequest.ToQueryString());
        }
    }
}
