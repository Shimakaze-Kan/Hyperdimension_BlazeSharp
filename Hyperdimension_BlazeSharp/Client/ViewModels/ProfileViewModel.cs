using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class ProfileViewModel : IProfileViewModel
    {
        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Banner { get; set; }
        public UserPreferences UserPreferences { get; set; }
        private readonly HttpClient _httpClient;

        public ProfileViewModel() { }                
        public ProfileViewModel(HttpClient httpClient) => _httpClient = httpClient;                 

        public async Task GetBanner()
        {
            var banners = await _httpClient.GetFromJsonAsync<string[]>("img/bg-banners.json");
            Banner = banners[new Random().Next(0, banners.Length)];
        }

        public async Task GetProfileData()
        {
            var userProfile = await _httpClient.GetFromJsonAsync<UserProfile>($"users/profile/{UserId}");
            LoadCurrentObject(userProfile);
        }

        public async Task<HttpResponseMessage> UpdatePreferences()
        {
            var result = await _httpClient.PostAsJsonAsync<UserPreferences>($"users/changeuserpreferences", UserPreferences);

            if(result.IsSuccessStatusCode)
            {
                UserProfile = UserProfile with { About = UserPreferences.About };
            }

            return result;
        }

        private void LoadCurrentObject(ProfileViewModel profileViewModel)
        {
            this.UserProfile = profileViewModel.UserProfile;
            this.Banner = profileViewModel.Banner;
        }

        public static implicit operator ProfileViewModel(UserProfile userProfile)
        {
            return new() { UserProfile = userProfile };
        }

        public static implicit operator ProfileViewModel(string banner)
        {
            return new() { Banner = banner };
        }
    }
}
