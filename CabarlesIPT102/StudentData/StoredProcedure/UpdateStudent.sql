CREATE PROCEDURE [dbo].[UpdateStudent]
    @StudentId INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Age INT,
    @Course NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [dbo].[Student]
    SET FirstName = @FirstName,
        LastName = @LastName,
        Age = @Age,
        Course = @Course
    WHERE StudentId = @StudentId;
END
