using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Newtonsoft.Json;
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

        public async Task<List<(int Index, string Word)>> ValidateComment(CommentType commentType)
        {
            var wordsList = await _httpClient.GetFromJsonAsync<List<string>>("profanityList/words.json");

            List<(int Index, string Word)> result = new();
            if (wordsList is null)
            {
                return result;
            }

            if (commentType == CommentType.Comment)
            {
                GetProhibitedWordList(CommentCreateRequest.Text, wordsList, result);
            }
            else if (commentType == CommentType.Subcomment)
            {
                GetProhibitedWordList(SubcommentCreateRequest.Text, wordsList, result);
            }

            return result;
        }

        private void GetProhibitedWordList(string text, List<string> wordsList, List<(int Index, string Word)> result)
        {
            var commonWords = text.Split(' ').Select(x => x.ToLower()).Intersect(wordsList);
            if (commonWords.Count() > 0)
            {
                foreach (var word in commonWords)
                {
                    result.Add((text.IndexOf(word), word));
                }
            }
        }
    }

    public enum CommentType
    {
        Comment,
        Subcomment
    }
}
