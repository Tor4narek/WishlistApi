using Moq;
using NUnit.Framework;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Services.Presenters;
using Services.Services;

namespace PresenterTests
{
    [TestFixture]
    public class UserPresenterTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthenticationService> _authServiceMock;
        private UserPresenter _userPresenter;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthenticationService>();
            _userPresenter = new UserPresenter(_userRepositoryMock.Object, _authServiceMock.Object);
        }

        [Test]
        public async Task CreateUserAsync_ShouldCallAuthServiceRegisterUserAsync()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string password = "securePassword";
            var cancellationToken = CancellationToken.None;

            // Act
            await _userPresenter.CreateUserAsync(name, email, password, cancellationToken);

            // Assert
            _authServiceMock.Verify(s => s.RegisterUserAsync(name, email, password, cancellationToken), Times.Once);
        }

        [Test]
        public async Task AuthenticateUserAsync_ShouldCallAuthServiceAuthenticateUserAsync_AndReturnUser()
        {
            // Arrange
            string email = "john@example.com";
            string password = "securePassword";
            var cancellationToken = CancellationToken.None;

            var expectedUser = new User { Id = "1", Name = "John Doe", Email = email };
            _authServiceMock.Setup(s => s.AuthenticateUserAsync(email, password, cancellationToken)).Returns(Task.CompletedTask);
            _authServiceMock.Setup(s => s.GetAuthenticatedUserAsync()).ReturnsAsync(expectedUser);

            // Act
            var user = await _userPresenter.AuthenticateUserAsync(email, password, cancellationToken);

            // Assert
            _authServiceMock.Verify(s => s.AuthenticateUserAsync(email, password, cancellationToken), Times.Once);
            _authServiceMock.Verify(s => s.GetAuthenticatedUserAsync(), Times.Once);
            Assert.AreEqual(expectedUser, user);
        }

        [Test]
        public async Task LoadUserAsync_ShouldReturnUserFromRepository()
        {
            // Arrange
            var userId = "1";
            var cancellationToken = CancellationToken.None;
            var expectedUser = new User { Id = userId, Name = "John Doe" };

            _userRepositoryMock.Setup(r => r.GetUserAsync(userId, cancellationToken)).ReturnsAsync(expectedUser);

            // Act
            var user = await _userPresenter.LoadUserAsync(userId, cancellationToken);

            // Assert
            _userRepositoryMock.Verify(r => r.GetUserAsync(userId, cancellationToken), Times.Once);
            Assert.AreEqual(expectedUser, user);
        }

        [Test]
        public async Task SearchUsersByKeywordAsync_WhenKeywordIsNullOrWhiteSpace_ShouldReturnEmptyList()
        {
            // Arrange
            string keyword = " ";
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _userPresenter.SearchUsersByKeywordAsync(keyword, cancellationToken);

            // Assert
            Assert.IsEmpty(result);
            _userRepositoryMock.Verify(r => r.SearchUsersByKeywordAsync(It.IsAny<string>(), cancellationToken), Times.Never);
        }

        [Test]
        public async Task SearchUsersByKeywordAsync_ShouldReturnUsersFromRepository()
        {
            // Arrange
            string keyword = "John";
            var cancellationToken = CancellationToken.None;
            var expectedUsers = new List<User>
            {
                new User { Id = "1", Name = "John Doe" },
                new User { Id = "2", Name = "Johnny Appleseed" }
            };

            _userRepositoryMock.Setup(r => r.SearchUsersByKeywordAsync(keyword, cancellationToken)).ReturnsAsync(expectedUsers);

            // Act
            var users = await _userPresenter.SearchUsersByKeywordAsync(keyword, cancellationToken);

            // Assert
            _userRepositoryMock.Verify(r => r.SearchUsersByKeywordAsync(keyword, cancellationToken), Times.Once);
            Assert.AreEqual(expectedUsers, users);
        }

        [Test]
        public async Task GetUserByEmailAsync_ShouldReturnUserFromRepository()
        {
            // Arrange
            string email = "john@example.com";
            var cancellationToken = CancellationToken.None;
            var expectedUser = new User { Id = "1", Name = "John Doe", Email = email };

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email, cancellationToken)).ReturnsAsync(expectedUser);

            // Act
            var user = await _userPresenter.GetUserByEmailAsync(email, cancellationToken);

            // Assert
            _userRepositoryMock.Verify(r => r.GetUserByEmailAsync(email, cancellationToken), Times.Once);
            Assert.AreEqual(expectedUser, user);
        }

        [Test]
        public async Task LogoutAsync_ShouldCallAuthServiceLogoutAsync()
        {
            // Act
            await _userPresenter.LogoutAsync();

            // Assert
            _authServiceMock.Verify(s => s.LogoutAsync(), Times.Once);
        }
    }
}
