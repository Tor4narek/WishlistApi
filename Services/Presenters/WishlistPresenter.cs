using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public class WishlistPresenter : IWishlistPresenter
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IUserPresenter _userPresenter;

        public WishlistPresenter()
        {
            _wishlistRepository = new WishlistRepository();
            _userPresenter = new UserPresenter();
        }
        public WishlistPresenter(IWishlistRepository wishlistRepository,IUserPresenter userPresenter)
        {
            _wishlistRepository = wishlistRepository;
            _userPresenter = userPresenter;
        }

        // Метод для загрузки всех вишлистов пользователя
        public async Task<IReadOnlyCollection<Wishlist>> LoadUserWishlistsAsync(string userId, CancellationToken token)
        {
          
            // Проверка отмены
            token.ThrowIfCancellationRequested();

            User user = await _userPresenter.LoadUserAsync(userId, token);
          

            var wishlists = await _wishlistRepository.GetUserWishlistsAsync(userId,token);
            return wishlists;
        }

        // Метод для добавления нового вишлиста

        public async Task AddNewWishlistAsync(string w_name, string w_description, string w_ownerId, string w_presentsNumber, CancellationToken token)
        {
            

            string w_id = Guid.NewGuid().ToString();
            var wishlist = new Wishlist(w_id, w_name, w_description, w_ownerId, w_presentsNumber);
            token.ThrowIfCancellationRequested();
            await _wishlistRepository.AddWishlistAsync(wishlist,token);
        }

        // Метод для удаления вишлиста по его ID
        public async Task DeleteWishlistAsync(string wishlistId, CancellationToken token)
        {
           

            token.ThrowIfCancellationRequested();
            await _wishlistRepository.DeleteWishlistAsync(wishlistId.ToString(), token);
        }

        public async Task UpdateWishlistAsync(Wishlist wishlist, string w_presentsNumber, CancellationToken token)
        {
           
               
          
            var newWishlist = new Wishlist(wishlist.Id, wishlist.Name, wishlist.Description, wishlist.OwnerId, w_presentsNumber);
            token.ThrowIfCancellationRequested();
            await _wishlistRepository.UpdateWishlistAsync(newWishlist,token);
            
        }
    }
}