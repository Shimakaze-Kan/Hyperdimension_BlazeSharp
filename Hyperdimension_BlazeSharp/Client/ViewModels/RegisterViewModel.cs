using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class RegisterViewModel : IRegisterViewModel
    {
        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(255), MinLength(5)]
        public string Password { get; set; }
        private readonly HttpClient _httpClient;

        public RegisterViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterUser()
        {
            return await _httpClient.PostAsJsonAsync<UserAuthenticationMinimal>("users/registeruser", this);
        }

        public static implicit operator UserAuthenticationMinimal(RegisterViewModel registerViewModel)
        {
            return new(registerViewModel.Email, registerViewModel.Password);
        }
    }
}
