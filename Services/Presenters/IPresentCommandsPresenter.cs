using Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public interface IPresentCommandsPresenter
    {
        Task AddNewPresentAsync(string name, string description, string reserverId, string wishlistId, CancellationToken token);
        Task DeletePresentAsync(string presentId, CancellationToken token);  // Заменил Guid на string
        Task ReservePresentAsync(string presentId, string reserverId, CancellationToken token);  // Заменил Guid на string
    }
}