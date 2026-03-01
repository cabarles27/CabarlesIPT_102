using Domain.Models;
using Domain.Queries;
using Repository.Interfaces;

namespace Framework.Queries;

public class ReadStudentById : IReadStudentById
{
    private readonly IRepository _repository;

    public ReadStudentById(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentModel?> ExecuteAsync(int studentId)
    {
        var parameters = new { StudentId = studentId };
        var result = await _repository.GetDataAsync<StudentModel>("ReadStudentById", parameters);
        return result.FirstOrDefault();
    }
}
