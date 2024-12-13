using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Presenters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wishlist.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistPresenter _wishlistPresenter;

        public WishlistController(IWishlistPresenter wishlistPresenter)
        {
            _wishlistPresenter = wishlistPresenter;
        }

        // Загрузка всех вишлистов пользователя
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWishlists(string userId, CancellationToken token)
        {
            try
            {
                var wishlists = await _wishlistPresenter.LoadUserWishlistsAsync(userId, token);
                return Ok(wishlists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Добавление нового вишлиста
        [HttpPost("add")]
        public async Task<IActionResult> AddWishlist([FromBody] WishlistCreationModel model, CancellationToken token)
        {
            if (model == null)
            {
                return BadRequest("Invalid wishlist data.");
            }

            try
            {
                await _wishlistPresenter.AddNewWishlistAsync(
                    model.Name, 
                    model.Description, 
                    model.OwnerId, 
                    model.PresentsNumber, 
                    token);

                return Ok(new { message = "Wishlist successfully created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Удаление вишлиста
        [HttpDelete("{wishlistId}")]
        public async Task<IActionResult> DeleteWishlist(string wishlistId, CancellationToken token)
        {
            try
            {
                await _wishlistPresenter.DeleteWishlistAsync(wishlistId, token);
                return Ok(new { message = "Wishlist successfully deleted" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Обновление вишлиста
        [HttpPut("update")]
        public async Task<IActionResult> UpdateWishlist([FromBody] WishlistUpdateModel model, CancellationToken token)
        {
            if (model == null)
            {
                return BadRequest("Invalid wishlist update data.");
            }

            try
            {
                var updatedWishlist = new Models.Wishlist(
                    model.Id, 
                    model.Name, 
                    model.Description, 
                    model.OwnerId, 
                    model.PresentsNumber);

                await _wishlistPresenter.UpdateWishlistAsync(updatedWishlist, model.PresentsNumber, token);
                return Ok(new { message = "Wishlist successfully updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}