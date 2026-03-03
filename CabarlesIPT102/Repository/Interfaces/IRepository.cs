namespace Repository.Interfaces;

public interface IRepository
{
    Task<bool> SaveDataAsync(string storedProcedure, object parameters);
    Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedure, object? parameters = null);
}
