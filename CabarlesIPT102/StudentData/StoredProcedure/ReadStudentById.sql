CREATE PROCEDURE [dbo].[ReadStudentById]
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT StudentId, FirstName, LastName, Age, Course
    FROM [dbo].[Student]
    WHERE StudentId = @StudentId;
END
