﻿using Hyperdimension_BlazeSharp.Client.ExtensionMethods;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class ProfileViewModel : ObservableObject, IProfileViewModel
    {
        private UserProfile _userProfile;
        private UserPreferencesForce _userPreferences;
        private string _banner;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;                

        public Guid? UserId { get; set; }
        public UserProfile UserProfile { get => _userProfile; set => OnPropertyChanged(ref _userProfile, value); }
        public string Banner { get => _banner; set => OnPropertyChanged(ref _banner, value); }
        public UserPreferencesForce UserPreferences { get => _userPreferences; set => OnPropertyChanged(ref _userPreferences, value); }

        public ProfileViewModel(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task GetBanner()
        {
            var banners = await _httpClient.GetFromJsonAsync<string[]>("img/bg-banners.json");
            Banner = banners[new Random().Next(0, banners.Length)];
        }

        public async Task GetProfileData()
        {
            UserProfile = await _httpClient.GetFromJsonAsync<UserProfile>($"users/profile/{UserId}");
        }

        public async Task<HttpResponseMessage> UpdatePreferences()
        {
            var result = await _httpClient.PutAsJsonAsyncJwtHeader(_localStorageService,UserPreferences,$"users/changeuserpreferences");


            if(result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(UserPreferences.AvatarUrl))
                {
                    UserProfile = UserProfile with { About = UserPreferences.About, AvatarUrl = UserPreferences.AvatarUrl };
                }
                else
                {
                    UserProfile = UserProfile with { About = UserPreferences.About };
                }
            }

            return result;
        }

        public async Task<HttpResponseMessage> UpdatePreferencesForce()
        {
            var result = await _httpClient.PutAsJsonAsyncJwtHeader(_localStorageService, UserPreferences, $"users/changeuserpreferencesforce");

            if (result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(UserPreferences.AvatarUrl))
                {
                    UserProfile = UserProfile with { About = UserPreferences.About, AvatarUrl = UserPreferences.AvatarUrl };
                }
                else
                {
                    UserProfile = UserProfile with { About = UserPreferences.About };
                }
            }

            return result;
        }

        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var buffer = new byte[file.Size];

            await file.OpenReadStream().ReadAsync(buffer);

            UserPreferences.AvatarUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        }
    }
}
