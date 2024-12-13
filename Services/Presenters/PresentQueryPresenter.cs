using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public class PresentQueryPresenter : IPresentQueryPresenter
    {
        private readonly IPresentRepository _presentRepository;

        public PresentQueryPresenter()
        {
            _presentRepository = new PresentRepository();
        }
        public PresentQueryPresenter(IPresentRepository presentRepository)
        {
            _presentRepository = presentRepository;
            
        }

        // Загрузка подарков из вишлиста по его ID
        public async Task<IReadOnlyCollection<Present>> LoadWishlistPresentsAsync(string wishlistId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();  // Проверка на отмену
            var presents = await _presentRepository.GetPresentsAsync(wishlistId, token);
            return presents;
        }

        public async Task<IReadOnlyCollection<Present>> LoadWishlistUnReservedPresentsAsync(string wishlistId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();  // Проверка на отмену
            var presents = await _presentRepository.GetUnReservedPresentsAsync(wishlistId, token);
            return presents;
        }

        // Поиск подарков по ключевому слову
        public async Task<IReadOnlyCollection<Present>> SearchPresentsByKeywordAsync(string keyword, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();  // Проверка на отмену
            var presents = await _presentRepository.SearchPresentsByKeywordAsync(keyword, token);
            return presents;
        }

        // Загрузка зарезервированных подарков для пользователя
        public async Task<IReadOnlyCollection<Present>> LoadReservedPresentsAsync(string userId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();  // Проверка на отмену
            var reservedPresents = await _presentRepository.GetReservedPresentsAsync(userId, token);
            return reservedPresents;
        }
    }
}
