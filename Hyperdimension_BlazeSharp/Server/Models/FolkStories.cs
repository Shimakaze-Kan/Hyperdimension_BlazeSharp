using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Server.Models
{
    public partial class FolkStories
    {
        public FolkStories()
        {
            Modules = new HashSet<Modules>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Story { get; set; }
        public string ImageUrl { get; set; }
        public string FolkStoriescol { get; set; }

        public virtual ICollection<Modules> Modules { get; set; }
    }
}
