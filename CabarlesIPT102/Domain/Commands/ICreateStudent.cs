using Domain.Models;

namespace Domain.Commands;

public interface ICreateStudent
{
    Task<bool> ExecuteAsync(StudentModel student);
}
