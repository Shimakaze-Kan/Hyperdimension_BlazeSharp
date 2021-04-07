using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class RankingViewModel : IRankingViewModel
    {
        public IEnumerable<UserRankingRecord> UserRankingRecords { get; set; }
        private readonly HttpClient _httpClient;

        public RankingViewModel() { }
        public RankingViewModel(HttpClient httpClient) => _httpClient = httpClient;               

        public async Task GetRankingList()
        {
            var rankingList = await _httpClient.GetFromJsonAsync<IEnumerable<UserRankingRecord>>("users/ranking");
            LoadCurrentObject(rankingList.ToList());
        }

        public static implicit operator RankingViewModel(List<UserRankingRecord> userRankingRecords)
            => new() { UserRankingRecords = userRankingRecords};

        private void LoadCurrentObject(RankingViewModel rankingViewModel)
            => this.UserRankingRecords = rankingViewModel.UserRankingRecords;
    }
}
