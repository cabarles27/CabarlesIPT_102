namespace Domain.Commands;

public interface IDeleteStudent
{
    Task<bool> ExecuteAsync(int studentId);
}
