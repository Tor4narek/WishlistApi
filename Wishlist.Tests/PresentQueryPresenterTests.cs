using Moq;
using NUnit.Framework;

using Models;
using Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.Presenters;

namespace PresenterTests
{
    [TestFixture]
    public class PresentQueryPresenterTests
    {
        private Mock<IPresentRepository> _presentRepositoryMock;
        private PresentQueryPresenter _presentQueryPresenter;

        [SetUp]
        public void Setup()
        {
            _presentRepositoryMock = new Mock<IPresentRepository>();
            _presentQueryPresenter = new PresentQueryPresenter(_presentRepositoryMock.Object);
        }

        [Test]
        public async Task LoadWishlistPresentsAsync_ShouldReturnPresentsFromRepository()
        {
            // Arrange
            string wishlistId = "wishlist1";
            var cancellationToken = CancellationToken.None;
            var expectedPresents = new List<Present>
            {
                new Present { Id = "1", Name = "Toy Car" },
                new Present { Id = "2", Name = "Doll House" }
            };

            _presentRepositoryMock
                .Setup(r => r.GetPresentsAsync(wishlistId, cancellationToken))
                .ReturnsAsync(expectedPresents);

            // Act
            var presents = await _presentQueryPresenter.LoadWishlistPresentsAsync(wishlistId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.GetPresentsAsync(wishlistId, cancellationToken), Times.Once);
            Assert.AreEqual(expectedPresents, presents);
        }

        [Test]
        public async Task LoadWishlistUnReservedPresentsAsync_ShouldReturnUnReservedPresentsFromRepository()
        {
            // Arrange
            string wishlistId = "wishlist1";
            var cancellationToken = CancellationToken.None;
            var expectedPresents = new List<Present>
            {
                new Present { Id = "3", Name = "Puzzle", IsReserved = false },
                new Present { Id = "4", Name = "Board Game", IsReserved = false }
            };

            _presentRepositoryMock
                .Setup(r => r.GetUnReservedPresentsAsync(wishlistId, cancellationToken))
                .ReturnsAsync(expectedPresents);

            // Act
            var presents = await _presentQueryPresenter.LoadWishlistUnReservedPresentsAsync(wishlistId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.GetUnReservedPresentsAsync(wishlistId, cancellationToken), Times.Once);
            Assert.AreEqual(expectedPresents, presents);
        }

        [Test]
        public async Task SearchPresentsByKeywordAsync_ShouldReturnPresentsMatchingKeyword()
        {
            // Arrange
            string keyword = "car";
            var cancellationToken = CancellationToken.None;
            var expectedPresents = new List<Present>
            {
                new Present { Id = "5", Name = "Toy Car" },
                new Present { Id = "6", Name = "Race Car" }
            };

            _presentRepositoryMock
                .Setup(r => r.SearchPresentsByKeywordAsync(keyword, cancellationToken))
                .ReturnsAsync(expectedPresents);

            // Act
            var presents = await _presentQueryPresenter.SearchPresentsByKeywordAsync(keyword, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.SearchPresentsByKeywordAsync(keyword, cancellationToken), Times.Once);
            Assert.AreEqual(expectedPresents, presents);
        }

        [Test]
        public async Task LoadReservedPresentsAsync_ShouldReturnReservedPresentsForUser()
        {
            // Arrange
            string userId = "user1";
            var cancellationToken = CancellationToken.None;
            var expectedReservedPresents = new List<Present>
            {
                new Present { Id = "7", Name = "Book", IsReserved = true },
                new Present { Id = "8", Name = "Pen", IsReserved = true }
            };

            _presentRepositoryMock
                .Setup(r => r.GetReservedPresentsAsync(userId, cancellationToken))
                .ReturnsAsync(expectedReservedPresents);

            // Act
            var reservedPresents = await _presentQueryPresenter.LoadReservedPresentsAsync(userId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.GetReservedPresentsAsync(userId, cancellationToken), Times.Once);
            Assert.AreEqual(expectedReservedPresents, reservedPresents);
        }
    }
}
