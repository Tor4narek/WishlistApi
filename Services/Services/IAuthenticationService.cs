using Models;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(string name, string email, string password, CancellationToken token);
        Task AuthenticateUserAsync(string email, string password, CancellationToken token);
        Task<User> GetAuthenticatedUserAsync();
        Task LogoutAsync();
    }
}