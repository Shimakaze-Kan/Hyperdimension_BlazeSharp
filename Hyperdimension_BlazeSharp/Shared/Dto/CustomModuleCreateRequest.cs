using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class CustomModuleCreateRequest
    {
        private string _folkStoryTitle;
        private string _folkStoryStory;
        private string _folkStoryImageUrl;

        [Required]
        public string Title { get; set; }
        [Required]
        public int Mode { get; set; }
        [Required]
        public string FolkStoryTitle { get => _folkStoryTitle; set { _folkStoryTitle = value; IsFolkStory = true; } }
        [Required]
        public string FolkStoryStory { get => _folkStoryStory; set { _folkStoryStory = value; IsFolkStory = true; } }
        public string FolkStoryImageUrl { get => _folkStoryImageUrl; set { _folkStoryImageUrl = value; IsFolkStory = true; } }

        public bool IsFolkStory { get; set; } = false;
    }
}
