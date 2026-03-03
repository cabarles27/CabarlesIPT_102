using Dapper;
using Domain.Models;

namespace Framework.Extensions;

public static class StudentExtension
{
    public static DynamicParameters ToStudentDynamicParameters(this StudentModel student)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@StudentId", student.StudentId);
        parameters.Add("@FirstName", student.FirstName);
        parameters.Add("@LastName", student.LastName);
        parameters.Add("@Age", student.Age);
        parameters.Add("@Course", student.Course);
        return parameters;
    }

    public static DynamicParameters ToCreateStudentDynamicParameters(this StudentModel student)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", student.FirstName);
        parameters.Add("@LastName", student.LastName);
        parameters.Add("@Age", student.Age);
        parameters.Add("@Course", student.Course);
        return parameters;
    }

    public static DynamicParameters ToDeleteStudentDynamicParameters(this int studentId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@StudentId", studentId);
        return parameters;
    }
}
