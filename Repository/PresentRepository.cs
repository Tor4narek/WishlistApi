using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Repository
{
    public class PresentRepository : IPresentRepository
    {
        private readonly IDatabaseRepository<Present> _repository;

        // Конструктор для инициализации PresentRepository с экземпляром DatabaseRepository
        public PresentRepository(IDatabaseRepository<Present> repository)
        {
            _repository = repository;
        }

        // Конструктор с жестко заданной строкой подключения и именем таблицы
        public PresentRepository()
        {
            _repository = new DatabaseRepository<Present>(
                "Host=80.64.24.84;Port=5432;Username=admin;Password=12345;Database=wishlistdb",
                "Presents");
        }

        // Получение всех подарков для конкретного вишлиста
        public async Task<IReadOnlyCollection<Present>> GetPresentsAsync(string wishlistId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Запрос для получения всех подарков с конкретным WishlistId
            var query = @"
                SELECT Id, Name, Description, WishlistId, IsReserved, ReserverId 
                FROM Presents 
                WHERE WishlistId = @WishlistId";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Выполнение запроса и возврат списка подарков
            var presents = await connection.QueryAsync<Present>(query, new { WishlistId = wishlistId });
            return presents.ToList();
        }

        public async Task<IReadOnlyCollection<Present>> GetUnReservedPresentsAsync(string wishlistId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Запрос для получения всех подарков с конкретным WishlistId
            var query = @"
                SELECT Id, Name, Description, WishlistId, IsReserved, ReserverId 
                FROM Presents 
                WHERE WishlistId = @WishlistId AND IsReserved = false";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Выполнение запроса и возврат списка подарков
            var presents = await connection.QueryAsync<Present>(query, new { WishlistId = wishlistId });
            return presents.ToList();
        }

        // Добавление нового подарка
        public async Task AddPresentAsync(Present present, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Запрос для вставки нового подарка
            var query = @"
                INSERT INTO Presents (Id, Name, Description, WishlistId, IsReserved, ReserverId) 
                VALUES (@Id, @Name, @Description, @WishlistId, @IsReserved, @ReserverId)";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Выполнение запроса на добавление
            await connection.ExecuteAsync(query, present);
        }

        // Удаление подарка по его Id
        public async Task DeletePresentAsync(string presentId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(presentId))
                throw new ArgumentException("PresentId cannot be empty.");

            // Запрос на удаление подарка по Id
            var query = "DELETE FROM Presents WHERE Id = @PresentId";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Выполнение запроса на удаление
            await connection.ExecuteAsync(query, new { PresentId = presentId });
        }

        // Поиск подарков по ключевому слову (показывать только незарезервированные)
        public async Task<IReadOnlyCollection<Present>> SearchPresentsByKeywordAsync(string keyword, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var query = @"
                SELECT Id, Name, Description, WishlistId, IsReserved, ReserverId 
                FROM Presents 
                WHERE (Name ILIKE @Keyword OR Description ILIKE @Keyword) 
                AND IsReserved = FALSE";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Выполнение запроса на поиск
            var presents = await connection.QueryAsync<Present>(query, new { Keyword = $"%{keyword}%" });
            return presents.ToList();
        }

        // Резервирование подарка
        public async Task ReservePresentAsync(string presentId, string reserverId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var query = @"
                SELECT Id, Name, Description, WishlistId, IsReserved, ReserverId 
                FROM Presents 
                WHERE Id = @PresentId";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Получение подарка из базы
            var present = await connection.QueryFirstOrDefaultAsync<Present>(query, new { PresentId = presentId });


            // Обновление статуса подарка на зарезервированный
            var updateQuery = @"
                UPDATE Presents 
                SET IsReserved = TRUE, ReserverId = @ReserverId 
                WHERE Id = @PresentId";

            await connection.ExecuteAsync(updateQuery, new { ReserverId = reserverId, PresentId = presentId });
        }

        // Получение всех зарезервированных подарков пользователя
        public async Task<IReadOnlyCollection<Present>> GetReservedPresentsAsync(string userId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var query = @"
                SELECT Id, Name, Description, WishlistId, IsReserved, ReserverId 
                FROM Presents 
                WHERE IsReserved = TRUE AND ReserverId = @UserId";

            using var connection = new NpgsqlConnection(_repository.GetConnectionString());
            await connection.OpenAsync(token);

            // Получение всех зарезервированных подарков для пользователя
            var presents = await connection.QueryAsync<Present>(query, new { UserId = userId });
            return presents.ToList();
        }
    }
}
