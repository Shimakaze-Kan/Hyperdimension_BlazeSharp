using Hyperdimension_BlazeSharp.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface ICustomModuleViewModel
    {
        public CustomModuleCreateRequest CreateRequest { get; set; }

        public Task<HttpResponseMessage> CreateModule();
    }
}
