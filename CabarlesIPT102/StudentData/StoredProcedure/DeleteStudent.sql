CREATE PROCEDURE [dbo].[DeleteStudent]
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM [dbo].[Student]
    WHERE StudentId = @StudentId;
END
