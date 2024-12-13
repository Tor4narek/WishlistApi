using Moq;
using NUnit.Framework;
using Repository;
using Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<IDatabaseRepository<User>> _databaseRepositoryMock;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _databaseRepositoryMock = new Mock<IDatabaseRepository<User>>();
            _userRepository = new UserRepository(_databaseRepositoryMock.Object);
        }

        [Test]
        public async Task GetUserAsync_ShouldReturnUser()
        {
            // Arrange
            string userId = "user123";
            var cancellationToken = CancellationToken.None;
            var expectedUser = new User { Id = userId, Name = "John Doe", Email = "john@example.com" };

            _databaseRepositoryMock
                .Setup(repo => repo.GetSingleAsync(
                    "id = @Id",
                    It.Is<object>(o => o.GetType().GetProperty("Id").GetValue(o).ToString() == userId),
                    cancellationToken))
                .ReturnsAsync(expectedUser);

            // Act
            var user = await _userRepository.GetUserAsync(userId, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo =>
                repo.GetSingleAsync(
                    "id = @Id",
                    It.Is<object>(o => o.GetType().GetProperty("Id").GetValue(o).ToString() == userId),
                    cancellationToken), Times.Once);
            Assert.AreEqual(expectedUser, user);
        }

        [Test]
        public async Task SearchUsersByKeywordAsync_ShouldReturnMatchingUsers()
        {
            // Arrange
            string keyword = "John";
            var cancellationToken = CancellationToken.None;
            var expectedUsers = new List<User>
            {
                new User { Id = "1", Name = "John Doe", Email = "john@example.com" },
                new User { Id = "2", Name = "Johnny Appleseed", Email = "johnny@example.com" }
            };

            _databaseRepositoryMock
                .Setup(repo => repo.GetListAsync(
                    "name ILIKE @Keyword OR email ILIKE @Keyword",
                    It.Is<object>(o => o.GetType().GetProperty("Keyword").GetValue(o).ToString() == $"%{keyword}%"),
                    cancellationToken))
                .ReturnsAsync(expectedUsers);

            // Act
            var users = await _userRepository.SearchUsersByKeywordAsync(keyword, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo =>
                repo.GetListAsync(
                    "name ILIKE @Keyword OR email ILIKE @Keyword",
                    It.Is<object>(o => o.GetType().GetProperty("Keyword").GetValue(o).ToString() == $"%{keyword}%"),
                    cancellationToken), Times.Once);
            Assert.AreEqual(expectedUsers, users);
        }

        [Test]
        public async Task GetUserByEmailAsync_ShouldReturnUser()
        {
            // Arrange
            string email = "john@example.com";
            var cancellationToken = CancellationToken.None;
            var expectedUser = new User { Id = "user123", Name = "John Doe", Email = email };

            _databaseRepositoryMock
                .Setup(repo => repo.GetSingleAsync(
                    "email = @Email",
                    It.Is<object>(o => o.GetType().GetProperty("Email").GetValue(o).ToString() == email),
                    cancellationToken))
                .ReturnsAsync(expectedUser);

            // Act
            var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo =>
                repo.GetSingleAsync(
                    "email = @Email",
                    It.Is<object>(o => o.GetType().GetProperty("Email").GetValue(o).ToString() == email),
                    cancellationToken), Times.Once);
            Assert.AreEqual(expectedUser, user);
        }

        [Test]
        public async Task AddUserAsync_ShouldCallAddAsync()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var newUser = new User { Id = "user123", Name = "John Doe", Email = "john@example.com" };

            // Act
            await _userRepository.AddUserAsync(newUser, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo => repo.AddAsync(newUser, cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldCallDeleteAsync()
        {
            // Arrange
            string userId = "user123";
            var cancellationToken = CancellationToken.None;

            // Act
            await _userRepository.DeleteUserAsync(userId, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo =>
                repo.DeleteAsync(
                    "id = @Id",
                    It.Is<object>(o => o.GetType().GetProperty("Id").GetValue(o).ToString() == userId),
                    cancellationToken), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldCallUpdateAsync()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var updatedUser = new User { Id = "user123", Name = "John Doe Updated", Email = "john@example.com" };

            // Act
            await _userRepository.UpdateUserAsync(updatedUser, cancellationToken);

            // Assert
            _databaseRepositoryMock.Verify(repo =>
                repo.UpdateAsync(
                    "id = @Id",
                    It.Is<object>(o => o.GetType().GetProperty("Id").GetValue(o).ToString() == updatedUser.Id),
                    updatedUser,
                    cancellationToken), Times.Once);
        }
    }
}
