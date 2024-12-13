using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
namespace Repository;

public interface IPresentRepository
{
    Task<IReadOnlyCollection<Present>> GetPresentsAsync(string wishlistId, CancellationToken token);
    Task<IReadOnlyCollection<Present>> GetUnReservedPresentsAsync(string wishlistId, CancellationToken token);
    Task AddPresentAsync(Present present, CancellationToken token);
    Task DeletePresentAsync(string presentId, CancellationToken token);
    Task<IReadOnlyCollection<Present>> SearchPresentsByKeywordAsync(string keyword, CancellationToken token);
    Task ReservePresentAsync(string presentId, string reserverId, CancellationToken token);
    Task<IReadOnlyCollection<Present>> GetReservedPresentsAsync(string userId, CancellationToken token);
}
