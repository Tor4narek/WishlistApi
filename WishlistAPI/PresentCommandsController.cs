using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Presenters;
using System.Threading;
using System.Threading.Tasks;

namespace Wishlist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требуется авторизация для всех маршрутов
    public class PresentCommandsController : ControllerBase
    {
        private readonly IPresentCommandsPresenter _presentCommandsPresenter;

        public PresentCommandsController(IPresentCommandsPresenter presentCommandsPresenter)
        {
            _presentCommandsPresenter = presentCommandsPresenter;
        }

        /// <summary>
        /// Добавление нового подарка в вишлист.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddPresent([FromBody] AddPresentRequest request, CancellationToken token)
        {
            try
            {
                await _presentCommandsPresenter.AddNewPresentAsync(
                    request.Name,
                    request.Description,
                    request.ReserverId,
                    request.WishlistId,
                    token
                );
                return Ok(new { message = "Present added successfully" });
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
        /// Удаление подарка по ID.
        /// </summary>
        [HttpDelete("delete/{presentId}")]
        public async Task<IActionResult> DeletePresent(string presentId, CancellationToken token)
        {
            try
            {
                await _presentCommandsPresenter.DeletePresentAsync(presentId, token);
                return Ok(new { message = "Present deleted successfully" });
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
        /// Резервирование подарка.
        /// </summary>
        [HttpPost("reserve")]
        public async Task<IActionResult> ReservePresent([FromBody] ReservePresentRequest request, CancellationToken token)
        {
            try
            {
                await _presentCommandsPresenter.ReservePresentAsync(request.PresentId, request.ReserverId, token);
                return Ok(new { message = "Present reserved successfully" });
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

    // Модель для добавления подарка
    public class AddPresentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReserverId { get; set; }
        public string WishlistId { get; set; }
    }

    // Модель для резервирования подарка
    public class ReservePresentRequest
    {
        public string PresentId { get; set; }
        public string ReserverId { get; set; }
    }
}
