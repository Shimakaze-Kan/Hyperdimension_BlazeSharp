﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hyperdimension_BlazeSharp.Server.Models
{
    public partial class Subcomments
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public DateTime SubmittedAt { get; set; }

        public virtual Comments Comment { get; set; }
        public virtual Users User { get; set; }
    }
}
