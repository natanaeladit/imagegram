using Imagegram.Application.Common.Models;
using System.Threading.Tasks;

namespace Imagegram.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResult> CreateAccountAsync(string email, string password, string name);
        Task<bool> AccountExistsByEmailAsync(string email);
        Task<AuthResult> LoginAsync(string email, string password);
    }
}
