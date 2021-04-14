using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Application.Comments.Queries.GetCommentsFromPost
{
    public class CommentVm
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
