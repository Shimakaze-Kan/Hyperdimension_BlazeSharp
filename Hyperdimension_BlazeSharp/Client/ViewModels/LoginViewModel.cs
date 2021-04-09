using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        private readonly HttpClient _httpClient;


        public LoginViewModel() { }                
        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoginUser()
        {
            await _httpClient.PostAsJsonAsync<UserAuthenticationMinimal>("users/loginuser", this);
        }

        private void LoadCurrentObject(LoginViewModel loginViewModel)
        {
            this.Email = loginViewModel.Email;
            this.Password = loginViewModel.Password;
        }

        public static implicit operator LoginViewModel(UserAuthenticationMinimal userAuthenticationMinimal)
        {
            return new()
            {
                Email = userAuthenticationMinimal.Email,
                Password = userAuthenticationMinimal.Password
            };
        }

        public static implicit operator UserAuthenticationMinimal(LoginViewModel loginViewModel)
        {
            return new(loginViewModel.Email, loginViewModel.Password);
        }
    }
}
