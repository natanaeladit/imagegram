using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Domain.Entities
{
    public class Post
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
