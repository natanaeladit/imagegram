using Imagegram.Application.Common.Interfaces;
using Imagegram.Domain.Entities;
using Imagegram.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
