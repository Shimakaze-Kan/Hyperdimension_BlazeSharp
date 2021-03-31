using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Shared.Models
{
    public partial class UsersPreferences
    {
        public UsersPreferences()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string WebsiteTheme { get; set; }
        public string EditorTheme { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
