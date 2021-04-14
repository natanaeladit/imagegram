using Imagegram.Application.Comments.Queries.GetCommentsFromPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Application.Posts.Queries.GetAllPosts
{
    public class PostVm
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comments { get; set; }
        public IEnumerable<CommentVm> CommentList { get; set; }
        public int CommentsCount { get; set; }
    }
}
