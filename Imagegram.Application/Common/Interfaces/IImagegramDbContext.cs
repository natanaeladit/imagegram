using Imagegram.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Application.Common.Interfaces
{
    public interface IImagegramDbContext
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
