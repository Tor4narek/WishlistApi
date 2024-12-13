using Moq;
using NUnit.Framework;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Services.Presenters;

namespace PresenterTests
{
    [TestFixture]
    public class WishlistPresenterTests
    {
        private Mock<IWishlistRepository> _wishlistRepositoryMock;
        private Mock<IUserPresenter> _userPresenterMock;
        private WishlistPresenter _wishlistPresenter;

        [SetUp]
        public void Setup()
        {
            _wishlistRepositoryMock = new Mock<IWishlistRepository>();
            _userPresenterMock = new Mock<IUserPresenter>();
            _wishlistPresenter = new WishlistPresenter(_wishlistRepositoryMock.Object, _userPresenterMock.Object);
        }

        [Test]
        public async Task LoadUserWishlistsAsync_ShouldReturnWishlistsForUser()
        {
            // Arrange
            string userId = "123";
            var cancellationToken = CancellationToken.None;
            var expectedUser = new User { Id = userId, Name = "John Doe" };
            var expectedWishlists = new List<Wishlist>
            {
                new Wishlist("1", "Wishlist 1", "Description 1", userId, "5"),
                new Wishlist("2", "Wishlist 2", "Description 2", userId, "3")
            };

            _userPresenterMock.Setup(u => u.LoadUserAsync(userId, cancellationToken)).ReturnsAsync(expectedUser);
            _wishlistRepositoryMock.Setup(w => w.GetUserWishlistsAsync(userId, cancellationToken)).ReturnsAsync(expectedWishlists);

            // Act
            var wishlists = await _wishlistPresenter.LoadUserWishlistsAsync(userId, cancellationToken);

            // Assert
            _userPresenterMock.Verify(u => u.LoadUserAsync(userId, cancellationToken), Times.Once);
            _wishlistRepositoryMock.Verify(w => w.GetUserWishlistsAsync(userId, cancellationToken), Times.Once);
            Assert.AreEqual(expectedWishlists, wishlists);
        }

        [Test]
        public async Task AddNewWishlistAsync_ShouldAddWishlistToRepository()
        {
            // Arrange
            string w_name = "New Wishlist";
            string w_description = "Description";
            string w_ownerId = "123";
            string w_presentsNumber = "10";
            var cancellationToken = CancellationToken.None;

            _wishlistRepositoryMock
                .Setup(w => w.AddWishlistAsync(It.IsAny<Wishlist>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _wishlistPresenter.AddNewWishlistAsync(w_name, w_description, w_ownerId, w_presentsNumber, cancellationToken);

            // Assert
            _wishlistRepositoryMock.Verify(w => w.AddWishlistAsync(It.Is<Wishlist>(wishlist =>
                wishlist.Name == w_name &&
                wishlist.Description == w_description &&
                wishlist.OwnerId == w_ownerId &&
                wishlist.PresentsNumber == w_presentsNumber), cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteWishlistAsync_ShouldDeleteWishlistFromRepository()
        {
            // Arrange
            string wishlistId = "1";
            var cancellationToken = CancellationToken.None;

            _wishlistRepositoryMock
                .Setup(w => w.DeleteWishlistAsync(wishlistId, cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _wishlistPresenter.DeleteWishlistAsync(wishlistId, cancellationToken);

            // Assert
            _wishlistRepositoryMock.Verify(w => w.DeleteWishlistAsync(wishlistId, cancellationToken), Times.Once);
        }

        [Test]
        public async Task UpdateWishlistAsync_ShouldUpdateWishlistInRepository()
        {
            // Arrange
            var wishlist = new Wishlist("1", "Wishlist 1", "Description 1", "123", "5");
            string updatedPresentsNumber = "10";
            var cancellationToken = CancellationToken.None;

            _wishlistRepositoryMock
                .Setup(w => w.UpdateWishlistAsync(It.IsAny<Wishlist>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _wishlistPresenter.UpdateWishlistAsync(wishlist, updatedPresentsNumber, cancellationToken);

            // Assert
            _wishlistRepositoryMock.Verify(w => w.UpdateWishlistAsync(It.Is<Wishlist>(updatedWishlist =>
                updatedWishlist.Id == wishlist.Id &&
                updatedWishlist.Name == wishlist.Name &&
                updatedWishlist.Description == wishlist.Description &&
                updatedWishlist.OwnerId == wishlist.OwnerId &&
                updatedWishlist.PresentsNumber == updatedPresentsNumber), cancellationToken), Times.Once);
        }
    }
}
