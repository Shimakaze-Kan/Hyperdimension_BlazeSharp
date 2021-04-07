using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperdimension_BlazeSharp.Shared.Dto;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface IRankingViewModel
    {
        public IEnumerable<UserRankingRecord> UserRankingRecords { get; set; }

        public Task GetRankingList();
    }
}
