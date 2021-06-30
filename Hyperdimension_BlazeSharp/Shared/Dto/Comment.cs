using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Shared.Dto
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public IEnumerable<Subcomment> Subcomments { get; set; }
    }
}
