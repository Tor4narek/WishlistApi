using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using Dapper;


namespace Repository
{
    public class DatabaseRepository<T>:IDatabaseRepository<T>

    {
    private readonly string _connectionString;
    private readonly string _tableName;

    public DatabaseRepository(string connectionString, string tableName)
    {
        _connectionString = connectionString;
        _tableName = tableName;
    }

    public string GetConnectionString()
    {
          return _connectionString; 
    }

    // Получение записи по условию
    public async Task<T> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var query = $"SELECT * FROM {_tableName} WHERE {whereClause}";
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
    }

    // Получение списка записей по условию
    public async Task<IReadOnlyCollection<T>> GetListAsync(string whereClause, object parameters,
        CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var query = $"SELECT * FROM {_tableName} WHERE {whereClause}";
        await connection.OpenAsync(cancellationToken);
        var result = await connection.QueryAsync<T>(query, parameters);
        return result.ToList();
    }

    // Добавление записи в таблицу
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var insertQuery = GenerateInsertQuery(entity);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(insertQuery, entity);
    }

    // Удаление записей по условию
    public async Task DeleteAsync(string whereClause, object parameters, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var query = $"DELETE FROM {_tableName} WHERE {whereClause}";
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(query, parameters);
    }

    // Обновление записи по условию
    public async Task UpdateAsync(string whereClause, object parameters, T updatedEntity,
        CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var updateQuery = GenerateUpdateQuery(whereClause, updatedEntity);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(updateQuery, updatedEntity);
    }

    // Генерация SQL-запроса для вставки данных
    private string GenerateInsertQuery(T entity)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);
        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        return $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";
    }

    // Генерация SQL-запроса для обновления данных
    private string GenerateUpdateQuery(string whereClause, T entity)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => $"{p.Name} = @{p.Name}");
        var setClause = string.Join(", ", properties);

        return $"UPDATE {_tableName} SET {setClause} WHERE {whereClause}";
    }
    }
}
