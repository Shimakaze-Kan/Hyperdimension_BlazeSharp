using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IProfileViewModel : INotifyPropertyChanged
    {
        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Banner { get; set; }
        public UserPreferences UserPreferences { get; set; }

        public Task GetProfileData();
        public Task GetBanner();
        public Task<HttpResponseMessage> UpdatePreferences();
    }
}
