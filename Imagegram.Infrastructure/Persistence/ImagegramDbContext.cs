using Imagegram.Application.Common.Interfaces;
using Imagegram.Domain.Entities;
using Imagegram.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Imagegram.Infrastructure.Persistence
{
    public class ImagegramDbContext : IdentityDbContext<ApplicationUser>, IImagegramDbContext
    {
        public ImagegramDbContext()
        {
        }
        public ImagegramDbContext(DbContextOptions<ImagegramDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
    }
}
