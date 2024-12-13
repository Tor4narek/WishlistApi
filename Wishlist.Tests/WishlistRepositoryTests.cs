using NUnit.Framework;
using Moq;
using Repository;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Tests
{
    [TestFixture]
    public class WishlistRepositoryTests
    {
        private Mock<IDatabaseRepository<Wishlist>> _repositoryMock;
        private WishlistRepository _wishlistRepository;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IDatabaseRepository<Wishlist>>();
            _wishlistRepository = new WishlistRepository(_repositoryMock.Object);
        }
        

        [Test]
        public void AddWishlistAsync_ShouldThrowExceptionIfWishlistIsNull()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _wishlistRepository.AddWishlistAsync(null, cancellationToken));
        }

      

        [Test]
        public void DeleteWishlistAsync_ShouldThrowExceptionIfWishlistIdIsNull()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _wishlistRepository.DeleteWishlistAsync(null, cancellationToken));
        }
    }
}