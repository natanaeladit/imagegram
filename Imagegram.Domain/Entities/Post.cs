using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Domain.Entities
{
    public class Post
    {
        [Key]
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comments { get; set; }
        public virtual List<Comment> CommentList { get; set; }
    }
}
