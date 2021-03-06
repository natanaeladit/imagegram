using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Domain.Entities
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
    }
}
