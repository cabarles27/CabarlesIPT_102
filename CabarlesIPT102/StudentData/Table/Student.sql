CREATE TABLE [dbo].[Student]
(
    [StudentId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Age] INT NOT NULL,
    [Course] NVARCHAR(100) NOT NULL
)
