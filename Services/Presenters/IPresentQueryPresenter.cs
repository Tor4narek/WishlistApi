using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public interface IPresentQueryPresenter
    {
        Task<IReadOnlyCollection<Present>> LoadWishlistPresentsAsync(string wishlistId, CancellationToken token);
        Task<IReadOnlyCollection<Present>> LoadWishlistUnReservedPresentsAsync(string wishlistId, CancellationToken token);
        Task<IReadOnlyCollection<Present>> SearchPresentsByKeywordAsync(string keyword, CancellationToken token);
        Task<IReadOnlyCollection<Present>> LoadReservedPresentsAsync(string userId, CancellationToken token);
    }
}