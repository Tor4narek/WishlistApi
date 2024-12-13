using Models;
using Services.Services;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public class UserPresenter : IUserPresenter
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserRepository _userRepository;

        public UserPresenter(IUserRepository userRepositoryMock, IAuthenticationService authServiceMock)
        {
            _userRepository = userRepositoryMock;
            _authService = authServiceMock;
        }

        public UserPresenter()
        {
            _userRepository = new UserRepository();
            _authService = new AuthenticationService();
        }

        // Метод для создания нового пользователя
        public async Task CreateUserAsync(string name, string email, string password, CancellationToken token)
        {
            await _authService.RegisterUserAsync(name, email, password, token);
        }

        // Метод для аутентификации пользователя
        public async Task<User> AuthenticateUserAsync(string email, string password, CancellationToken token)
        {
            await _authService.AuthenticateUserAsync(email, password, token);
            var user = await _authService.GetAuthenticatedUserAsync();
            return user;
        }

        // Метод для загрузки пользователя по ID (изменен тип userId на string)
        public async Task<User> LoadUserAsync(string userId, CancellationToken token)
        {
            var user = await _userRepository.GetUserAsync(userId, token);
            return user;
        }

        public async Task<User> GetAuthenticatedUserAsync(CancellationToken token)
        {
            var user = await _authService.GetAuthenticatedUserAsync();
            return user;
        }

        public async Task<IReadOnlyCollection<User>> SearchUsersByKeywordAsync(string keyword, CancellationToken token)
        {
            // Проводим проверку входного параметра
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Array.Empty<User>(); // Возвращаем пустой массив, если ключевое слово пустое
            }
            // Получаем пользователей по ключевому слову
            var users = await _userRepository.SearchUsersByKeywordAsync(keyword, token);
            return users;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var user = await _userRepository.GetUserByEmailAsync(email, token);
            return user;
        }

        public async Task LogoutAsync()
        {
            await _authService.LogoutAsync();
        }
    }
}
