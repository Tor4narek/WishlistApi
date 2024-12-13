using Moq;
using NUnit.Framework;
using Services;
using Models;
using Repository;
using System.Threading;
using System.Threading.Tasks;
using Services.Presenters;

namespace PresenterTests
{
    [TestFixture]
    public class PresentCommandsPresenterTests
    {
        private Mock<IPresentRepository> _presentRepositoryMock;
        private PresentCommandsPresenter _presentCommandsPresenter;

        [SetUp]
        public void Setup()
        {
            _presentRepositoryMock = new Mock<IPresentRepository>();
            _presentCommandsPresenter = new PresentCommandsPresenter(_presentRepositoryMock.Object);
        }

        [Test]
        public async Task AddNewPresentAsync_ShouldCallAddPresentAsyncWithGeneratedId()
        {
            // Arrange
            string name = "Toy";
            string description = "A fun toy";
            string reserverId = "user123";
            string wishlistId = "wishlist123";
            var cancellationToken = CancellationToken.None;

            // Act
            await _presentCommandsPresenter.AddNewPresentAsync(name, description, reserverId, wishlistId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r =>
                r.AddPresentAsync(
                    It.Is<Present>(p =>
                        p.Name == name &&
                        p.Description == description &&
                        p.ReserverId == reserverId &&
                        p.WishlistId == wishlistId &&
                        !p.IsReserved &&
                        !string.IsNullOrEmpty(p.Id)), // Проверяем, что Id генерируется
                    cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeletePresentAsync_ShouldCallDeletePresentAsync()
        {
            // Arrange
            string presentId = "present123";
            var cancellationToken = CancellationToken.None;

            // Act
            await _presentCommandsPresenter.DeletePresentAsync(presentId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.DeletePresentAsync(presentId, cancellationToken), Times.Once);
        }

        [Test]
        public async Task ReservePresentAsync_ShouldCallReservePresentAsync()
        {
            // Arrange
            string presentId = "present123";
            string reserverId = "user123";
            var cancellationToken = CancellationToken.None;

            // Act
            await _presentCommandsPresenter.ReservePresentAsync(presentId, reserverId, cancellationToken);

            // Assert
            _presentRepositoryMock.Verify(r => r.ReservePresentAsync(presentId, reserverId, cancellationToken), Times.Once);
        }
    }
}
