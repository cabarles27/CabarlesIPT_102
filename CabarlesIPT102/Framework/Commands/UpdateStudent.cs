using Domain.Commands;
using Domain.Models;
using Framework.Extensions;
using Repository.Interfaces;

namespace Framework.Commands;

public class UpdateStudent : IUpdateStudent
{
    private readonly IRepository _repository;

    public UpdateStudent(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(StudentModel student)
    {
        var parameters = student.ToStudentDynamicParameters();
        return await _repository.SaveDataAsync("UpdateStudent", parameters);
    }
}
