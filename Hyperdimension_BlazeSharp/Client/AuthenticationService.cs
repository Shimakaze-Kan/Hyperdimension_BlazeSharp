using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _customAuthenticationState;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(HttpClient httpClient,
            AuthenticationStateProvider customAuthenticationState,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _customAuthenticationState = customAuthenticationState;
            _localStorageService = localStorageService;
        }


        public async Task<UserAuthResult> Login(UserAuthRequest userAuthRequest)
        {
            var data = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new("grant_type", "password"),
                new("email", userAuthRequest.Email),
                new("password", userAuthRequest.Password)
            });

            var result = await _httpClient.PostAsync("/token", data); //token only for testing purposes
            var resultContent = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var deserializedResult = JsonSerializer.Deserialize<UserAuthResult>(
                resultContent,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            await _localStorageService.SetItem("hbsToken", deserializedResult.Token);

            (_customAuthenticationState as CustomAuthenticationStateProvider).NotifyUserAuthentication(deserializedResult.Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", deserializedResult.Token);

            return deserializedResult;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItem("hbs");
            (_customAuthenticationState as CustomAuthenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
