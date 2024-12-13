using Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public interface IUserPresenter
    {
        Task<User> LoadUserAsync(string userId, CancellationToken token); // Изменен тип userId на string
        Task CreateUserAsync(string name, string email, string password, CancellationToken token);
        Task<User> AuthenticateUserAsync(string email, string password, CancellationToken token);
        Task<User> GetAuthenticatedUserAsync(CancellationToken token);
        Task<IReadOnlyCollection<User>> SearchUsersByKeywordAsync(string keyword, CancellationToken token);
        Task LogoutAsync();
    }
}