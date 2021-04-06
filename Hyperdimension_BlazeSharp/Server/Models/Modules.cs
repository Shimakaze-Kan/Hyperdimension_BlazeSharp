using Hyperdimension_BlazeSharp.Server.Models;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Server.Models
{
    public partial class Modules
    {
        public Modules()
        {
            Tasks = new HashSet<Tasks>();
        }

        public Guid Id { get; set; }
        public Guid? FolkStoryId { get; set; }
        public string Title { get; set; }
        public int Mode { get; set; }

        public virtual FolkStories FolkStory { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
