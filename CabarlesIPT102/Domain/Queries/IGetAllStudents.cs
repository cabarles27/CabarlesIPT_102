using Domain.Models;

namespace Domain.Queries;

public interface IGetAllStudents
{
    Task<IEnumerable<StudentModel>> ExecuteAsync();
}
