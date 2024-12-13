namespace Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

public interface IUserRepository
{
    Task<User> GetUserAsync(string userId, CancellationToken token);
    Task<IReadOnlyCollection<User>> SearchUsersByKeywordAsync(string keyword, CancellationToken token);
    Task AddUserAsync(User user, CancellationToken token);
    Task<User> GetUserByEmailAsync(string email, CancellationToken token);
    Task DeleteUserAsync(string userId, CancellationToken token);
    Task UpdateUserAsync(User user, CancellationToken token);
}