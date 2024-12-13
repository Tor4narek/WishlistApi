using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseRepository<User> _repository;

        public UserRepository()
        {
            _repository = new DatabaseRepository<User>("Host=80.64.24.84;Port=5432;Username=admin;Password=12345;Database=wishlistdb", "Users");
        }
        public UserRepository(IDatabaseRepository<User> repository)
        {
            _repository = repository;
        }

        // Получение пользователя по Id
        public async Task<User> GetUserAsync(string userId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _repository.GetSingleAsync("id = @Id", new { Id = userId }, token);
        }

        // Поиск пользователей по ключевому слову
        public async Task<IReadOnlyCollection<User>> SearchUsersByKeywordAsync(string keyword, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _repository.GetListAsync(
                "name ILIKE @Keyword OR email ILIKE @Keyword",
                new { Keyword = $"%{keyword}%" },
                token);
        }

        // Получение пользователя по email
        public async Task<User> GetUserByEmailAsync(string email, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await _repository.GetSingleAsync("email = @Email", new { Email = email }, token);
        }

        // Добавление нового пользователя
        public async Task AddUserAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _repository.AddAsync(user, token);
        }

        // Удаление пользователя по Id
        public async Task DeleteUserAsync(string userId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _repository.DeleteAsync("id = @Id", new { Id = userId }, token);
        }

        // Обновление пользователя по Id
        public async Task UpdateUserAsync(User updatedUser, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _repository.UpdateAsync("id = @Id", new { Id = updatedUser.Id }, updatedUser, token);
        }
    }
}
