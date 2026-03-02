using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace CabarlesWPF.Services;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Parse connection string to get server and database name
            var builder = new SqlConnectionStringBuilder(_connectionString);
            var serverConnection = builder.ConnectionString.Replace($"Database={builder.InitialCatalog};", "");
            var databaseName = builder.InitialCatalog;

            // Check if database exists
            using (var connection = new SqlConnection(serverConnection))
            {
                await connection.OpenAsync();

                var checkDbCommand = connection.CreateCommand();
                checkDbCommand.CommandText = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";
                var dbExists = await checkDbCommand.ExecuteScalarAsync();

                if (dbExists == null)
                {
                    // Create database
                    var createDbCommand = connection.CreateCommand();
                    createDbCommand.CommandText = $"CREATE DATABASE [{databaseName}]";
                    await createDbCommand.ExecuteNonQueryAsync();
                }
            }

            // Now connect to the database and create tables/procedures if needed
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check if Student table exists
                var checkTableCommand = connection.CreateCommand();
                checkTableCommand.CommandText = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Student'";
                var result = await checkTableCommand.ExecuteScalarAsync();
                var tableExists = result != null && (int)result > 0;

                if (!tableExists)
                {
                    await CreateSchemaAsync(connection);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            throw;
        }
    }

    private async Task CreateSchemaAsync(SqlConnection connection)
    {
        // Create Student table
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = @"
            CREATE TABLE Student (
                StudentId INT IDENTITY(1,1) PRIMARY KEY,
                FirstName NVARCHAR(100) NOT NULL,
                LastName NVARCHAR(100) NOT NULL,
                Age INT NOT NULL,
                Course NVARCHAR(100) NOT NULL
            );";
        await createTableCommand.ExecuteNonQueryAsync();

        // Create stored procedures
        await CreateStoredProceduresAsync(connection);

        // No sample data - empty database
    }

    private async Task CreateStoredProceduresAsync(SqlConnection connection)
    {
        var procedures = new[]
        {
            @"CREATE PROCEDURE CreateStudent
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @Age INT,
                @Course NVARCHAR(100)
              AS
              BEGIN
                INSERT INTO Student (FirstName, LastName, Age, Course)
                VALUES (@FirstName, @LastName, @Age, @Course);
              END",

            @"CREATE PROCEDURE UpdateStudent
                @StudentId INT,
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @Age INT,
                @Course NVARCHAR(100)
              AS
              BEGIN
                UPDATE Student
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Age = @Age,
                    Course = @Course
                WHERE StudentId = @StudentId;
              END",

            @"CREATE PROCEDURE DeleteStudent
                @StudentId INT
              AS
              BEGIN
                DELETE FROM Student WHERE StudentId = @StudentId;
              END",

            @"CREATE PROCEDURE GetAllStudents
              AS
              BEGIN
                SELECT StudentId, FirstName, LastName, Age, Course
                FROM Student
                ORDER BY StudentId;
              END",

            @"CREATE PROCEDURE ReadStudentById
                @StudentId INT
              AS
              BEGIN
                SELECT StudentId, FirstName, LastName, Age, Course
                FROM Student
                WHERE StudentId = @StudentId;
              END"
        };

        foreach (var procedureSql in procedures)
        {
            var command = connection.CreateCommand();
            command.CommandText = procedureSql;
            await command.ExecuteNonQueryAsync();
        }
    }
}
