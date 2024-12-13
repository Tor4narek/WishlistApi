using Models;
using Repository;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private UserRepository _userRepository = new UserRepository();
        private User _authenticatedUser = null;

        public async Task RegisterUserAsync(string name, string email, string password, CancellationToken token)
        {
            // Валидация данных
            ValidateUserData(name, email, password);

            // Проверка, существует ли пользователь с таким email
            var userEmail = await _userRepository.GetUserByEmailAsync(email, token);
            if (userEmail == null)
            {
                var id = Guid.NewGuid().ToString();
                await _userRepository.AddUserAsync(new User(id, name, email, HashPassword(password)), token);
            }
            else
            {
                throw new AuthenticationServiceException("The user already exists with this email");
            }
        }

        public async Task AuthenticateUserAsync(string email, string password, CancellationToken token)
        {
            // Валидация email и пароля
            ValidateEmailAndPassword(email, password);

            var user = await _userRepository.GetUserByEmailAsync(email, token);
            if (user != null)
            {
                if (user.PasswordHash == HashPassword(password))
                {
                    _authenticatedUser = user;
                }
                else
                {
                    throw new AuthenticationServiceException("The password does not match");
                }
            }
            else
            {
                throw new AuthenticationServiceException("The user does not exist");
            }
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            if (_authenticatedUser != null)
            {
                return _authenticatedUser;
            }
            
            return null;
            
        }

        public async Task LogoutAsync()
        {
            _authenticatedUser = null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void ValidateUserData(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new AuthenticationServiceException("Name cannot be empty");
            }

            if (!IsValidEmail(email))
            {
                throw new AuthenticationServiceException("Invalid email format");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                throw new AuthenticationServiceException("Password must be at least 6 characters long");
            }
        }

        private void ValidateEmailAndPassword(string email, string password)
        {
            if (!IsValidEmail(email))
            {
                throw new AuthenticationServiceException("Invalid email format");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AuthenticationServiceException("Password cannot be empty");
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

           
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
                return emailRegex.IsMatch(email);
            }
           
        }
    
}

