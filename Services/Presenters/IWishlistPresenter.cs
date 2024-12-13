using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Services.Presenters
{
    public interface IWishlistPresenter
    {
        Task<IReadOnlyCollection<Wishlist>> LoadUserWishlistsAsync(string userId, CancellationToken token); // Заменен тип userId на string

        Task AddNewWishlistAsync(string w_name, string w_description, string w_ownerId, string w_presentsNumber, CancellationToken token);
        
        Task DeleteWishlistAsync(string wishlistId, CancellationToken token); // Заменен тип wishlistId на string
        Task UpdateWishlistAsync(Wishlist wishlist, string w_presentsNumber, CancellationToken token);
    }
}