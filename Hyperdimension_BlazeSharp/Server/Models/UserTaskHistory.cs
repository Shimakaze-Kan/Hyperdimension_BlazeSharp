using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Server.Models
{
    public partial class UserTaskHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Solution { get; set; }
        public sbyte IsTaskPassed { get; set; }
        public DateTime SubmittedAt { get; set; }

        public virtual Tasks Task { get; set; }
        public virtual Users User { get; set; }
    }
}
