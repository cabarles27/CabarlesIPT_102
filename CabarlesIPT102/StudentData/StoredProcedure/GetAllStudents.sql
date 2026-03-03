CREATE PROCEDURE [dbo].[GetAllStudents]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT StudentId, FirstName, LastName, Age, Course
    FROM [dbo].[Student]
    ORDER BY StudentId;
END
