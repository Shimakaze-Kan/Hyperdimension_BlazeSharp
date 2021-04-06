using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IProfileViewModel
    {
        public Guid UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Banner { get; set; }

        public Task GetProfileData();
        public Task GetBanner();
    }
}
