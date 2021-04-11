using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace Hyperdimension_BlazeSharp.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _httpClient.GetFromJsonAsync<UserGuidEmail>("users/getcurrentuser");

            if (currentUser is not null && !string.IsNullOrEmpty(currentUser.Email))
            {
                var claimEmail = new Claim(ClaimTypes.Name, currentUser.Email);
                var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Guid));
                var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimNameIdentifier }, "ServerSideAuthentication");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
