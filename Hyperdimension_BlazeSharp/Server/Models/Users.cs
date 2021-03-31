using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Server.Models
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            UserTaskHistory = new HashSet<UserTaskHistory>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        public string Role { get; set; }
        public int UserPreferencesId { get; set; }

        public virtual UsersPreferences UserPreferences { get; set; }
        public virtual UsersDetails UsersDetails { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<UserTaskHistory> UserTaskHistory { get; set; }
    }
}
