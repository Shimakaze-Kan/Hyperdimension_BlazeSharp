using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Shared.Models
{
    public partial class UsersDetails
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string About { get; set; }
        public int Points { get; set; }
        public Guid UserId { get; set; }

        public virtual Users User { get; set; }
    }
}
