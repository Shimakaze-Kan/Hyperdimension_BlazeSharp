using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components.Forms;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IProfileViewModel : INotifyPropertyChanged
    {
        public Guid? UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Banner { get; set; }
        public UserPreferencesForce UserPreferences { get; set; }

        public Task GetProfileData();
        public Task GetBanner();
        public Task<HttpResponseMessage> UpdatePreferences();
        public Task<HttpResponseMessage> UpdatePreferencesForce();
        public Task OnInputFileChange(InputFileChangeEventArgs e);
    }
}
