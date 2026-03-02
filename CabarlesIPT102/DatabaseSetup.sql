-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'CabarlesIPT102')
BEGIN
    ALTER DATABASE CabarlesIPT102 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CabarlesIPT102;
END
GO

-- Create database
CREATE DATABASE CabarlesIPT102;
GO

USE CabarlesIPT102;
GO

-- Create Student table
CREATE TABLE Student (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Course NVARCHAR(100) NOT NULL
);
GO

-- Create stored procedure: CreateStudent
CREATE PROCEDURE CreateStudent
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Age INT,
    @Course NVARCHAR(100)
AS
BEGIN
    INSERT INTO Student (FirstName, LastName, Age, Course)
    VALUES (@FirstName, @LastName, @Age, @Course);
END
GO

-- Create stored procedure: UpdateStudent
CREATE PROCEDURE UpdateStudent
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
END
GO

-- Create stored procedure: DeleteStudent
CREATE PROCEDURE DeleteStudent
    @StudentId INT
AS
BEGIN
    DELETE FROM Student WHERE StudentId = @StudentId;
END
GO

-- Create stored procedure: GetAllStudents
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SELECT StudentId, FirstName, LastName, Age, Course
    FROM Student
    ORDER BY StudentId;
END
GO

-- Create stored procedure: ReadStudentById
CREATE PROCEDURE ReadStudentById
    @StudentId INT
AS
BEGIN
    SELECT StudentId, FirstName, LastName, Age, Course
    FROM Student
    WHERE StudentId = @StudentId;
END
GO

-- Insert sample data
INSERT INTO Student (FirstName, LastName, Age, Course) VALUES ('Juan', 'Dela Cruz', 20, 'Computer Science');
INSERT INTO Student (FirstName, LastName, Age, Course) VALUES ('Maria', 'Santos', 21, 'Information Technology');
INSERT INTO Student (FirstName, LastName, Age, Course) VALUES ('Pedro', 'Reyes', 19, 'Software Engineering');
GO

PRINT 'Database CabarlesIPT102 created successfully with 3 sample students!';
GO
