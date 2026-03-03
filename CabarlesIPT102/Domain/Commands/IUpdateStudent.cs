using Domain.Models;

namespace Domain.Commands;

public interface IUpdateStudent
{
    Task<bool> ExecuteAsync(StudentModel student);
}
