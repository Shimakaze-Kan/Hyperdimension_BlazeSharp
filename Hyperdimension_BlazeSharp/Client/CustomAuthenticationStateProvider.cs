using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationState _anonymous;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtEncodedString = await _localStorageService.GetItem<string>("hbsToken");

            if(string.IsNullOrWhiteSpace(jwtEncodedString))
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtEncodedString);

            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType")));

            //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(Jwt)))

            //var currentUser = await _httpClient.GetFromJsonAsync<UserGuidEmail>("users/getcurrentuser");

            //if (currentUser is not null && !string.IsNullOrEmpty(currentUser.Email))
            //{
            //    var claimEmail = new Claim(ClaimTypes.Name, currentUser.Email);
            //    var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Guid));
            //    var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimNameIdentifier }, "ServerSideAuthentication");
            //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //    return new AuthenticationState(claimsPrincipal);
            //}
            //else
            //{
            //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //}
        }

        public void NotifyUserAuthentication(string jwtEncodedString)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString[7..]);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);

            NotifyAuthenticationStateChanged(authState);
        }
    }
}
