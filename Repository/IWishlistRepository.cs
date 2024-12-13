using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public interface IWishlistRepository
    {
        Task<IReadOnlyCollection<Wishlist>> GetUserWishlistsAsync(string userId, CancellationToken token);
        Task AddWishlistAsync(Wishlist wishlist, CancellationToken token);
        Task DeleteWishlistAsync(string wishlistId, CancellationToken token);
        Task UpdateWishlistAsync(Wishlist wishlist, CancellationToken token);
    }
}