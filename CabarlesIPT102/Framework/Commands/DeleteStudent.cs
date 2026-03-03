using Domain.Commands;
using Framework.Extensions;
using Repository.Interfaces;

namespace Framework.Commands;

public class DeleteStudent : IDeleteStudent
{
    private readonly IRepository _repository;

    public DeleteStudent(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int studentId)
    {
        var parameters = studentId.ToDeleteStudentDynamicParameters();
        return await _repository.SaveDataAsync("DeleteStudent", parameters);
    }
}
