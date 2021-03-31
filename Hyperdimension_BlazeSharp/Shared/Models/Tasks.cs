using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Shared.Models
{
    public partial class Tasks
    {
        public Tasks()
        {
            Comments = new HashSet<Comments>();
            UserTaskHistory = new HashSet<UserTaskHistory>();
        }

        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InitialCode { get; set; }
        public string TestCode { get; set; }
        public int? Points { get; set; }

        public virtual Modules Module { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<UserTaskHistory> UserTaskHistory { get; set; }
    }
}
