using Domain.Models;
using Domain.Queries;
using Repository.Interfaces;

namespace Framework.Queries;

public class GetAllStudents : IGetAllStudents
{
    private readonly IRepository _repository;

    public GetAllStudents(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StudentModel>> ExecuteAsync()
    {
        return await _repository.GetDataAsync<StudentModel>("GetAllStudents");
    }
}
