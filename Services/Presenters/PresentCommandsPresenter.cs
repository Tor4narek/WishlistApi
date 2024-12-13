using Models;
using Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Presenters
{
    public class PresentCommandsPresenter : IPresentCommandsPresenter
    {
        private readonly IPresentRepository _presentRepository;

        public PresentCommandsPresenter()
        {
            _presentRepository = new PresentRepository();
        }

        public PresentCommandsPresenter(IPresentRepository presentRepository)
        {
            _presentRepository = presentRepository;
        }

        // Добавление нового подарка
        public async Task AddNewPresentAsync(string name, string description, string reserverId, string wishlistId, CancellationToken token)
        {
            string id = Guid.NewGuid().ToString(); // Генерация нового строкового идентификатора
            Present present = new Present(id, name, description, wishlistId, false, reserverId);
            
            token.ThrowIfCancellationRequested(); // Проверка на отмену
            await _presentRepository.AddPresentAsync(present, token);
        }

        // Удаление подарка по ID
        public async Task DeletePresentAsync(string presentId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested(); // Проверка на отмену
            await _presentRepository.DeletePresentAsync(presentId, token);
        }

        // Резервирование подарка
        public async Task ReservePresentAsync(string presentId, string reserverId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested(); // Проверка на отмену
            await _presentRepository.ReservePresentAsync(presentId, reserverId, token);
        }
    }
}