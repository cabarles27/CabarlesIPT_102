using Domain.Models;

namespace Domain.Queries;

public interface IReadStudentById
{
    Task<StudentModel?> ExecuteAsync(int studentId);
}
