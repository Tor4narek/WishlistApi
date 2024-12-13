using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Presenters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Wishlist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требуется авторизация для всех маршрутов
    public class PresentQueryController : ControllerBase
    {
        private readonly IPresentQueryPresenter _presentQueryPresenter;

        public PresentQueryController(IPresentQueryPresenter presentQueryPresenter)
        {
            _presentQueryPresenter = presentQueryPresenter;
        }

        /// <summary>
        /// Загрузка всех подарков из вишлиста по его ID.
        /// </summary>
        [HttpGet("wishlist/{wishlistId}")]
        public async Task<IActionResult> GetWishlistPresents(string wishlistId, CancellationToken token)
        {
            try
            {
                var presents = await _presentQueryPresenter.LoadWishlistPresentsAsync(wishlistId, token);
                return Ok(presents);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(499, "Request was canceled.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Загрузка незарезервированных подарков из вишлиста по его ID.
        /// </summary>
        [HttpGet("wishlist/{wishlistId}/unreserved")]
        public async Task<IActionResult> GetWishlistUnReservedPresents(string wishlistId, CancellationToken token)
        {
            try
            {
                var presents = await _presentQueryPresenter.LoadWishlistUnReservedPresentsAsync(wishlistId, token);
                return Ok(presents);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(499, "Request was canceled.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Поиск подарков по ключевому слову.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchPresents([FromQuery] string keyword, CancellationToken token)
        {
            try
            {
                var presents = await _presentQueryPresenter.SearchPresentsByKeywordAsync(keyword, token);
                return Ok(presents);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(499, "Request was canceled.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Загрузка зарезервированных подарков для пользователя по его ID.
        /// </summary>
        [HttpGet("reserved/{userId}")]
        public async Task<IActionResult> GetReservedPresents(string userId, CancellationToken token)
        {
            try
            {
                var reservedPresents = await _presentQueryPresenter.LoadReservedPresentsAsync(userId, token);
                return Ok(reservedPresents);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(499, "Request was canceled.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
