using Domain.Commands;
using Domain.Models;
using Framework.Extensions;
using Repository.Interfaces;

namespace Framework.Commands;

public class CreateStudent : ICreateStudent
{
    private readonly IRepository _repository;

    public CreateStudent(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(StudentModel student)
    {
        var parameters = student.ToCreateStudentDynamicParameters();
        return await _repository.SaveDataAsync("CreateStudent", parameters);
    }
}
