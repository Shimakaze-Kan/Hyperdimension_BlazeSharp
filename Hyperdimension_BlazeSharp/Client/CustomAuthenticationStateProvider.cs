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
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtEncodedString = await _localStorageService.GetItem<string>("hbsToken");

            if(string.IsNullOrWhiteSpace(jwtEncodedString))
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtEncodedString);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtEncodedString);
            var tokenS = jsonToken as JwtSecurityToken;            

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity((List<Claim>)tokenS.Claims, "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string jwtEncodedString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtEncodedString);
            var tokenS = jsonToken as JwtSecurityToken;

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity((List<Claim>)tokenS.Claims, "jwtAuthType"));
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
