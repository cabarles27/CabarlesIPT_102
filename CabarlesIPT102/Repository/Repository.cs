using Dapper;
using Microsoft.Data.SqlClient;
using Repository.Interfaces;
using System.Data;

namespace Repository;

public class Repository : IRepository
{
    private readonly string _connectionString;

    public Repository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> SaveDataAsync(string storedProcedure, object parameters)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedure, object? parameters = null)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
