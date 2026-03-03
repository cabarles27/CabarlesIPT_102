param(
    [Parameter(Mandatory=$false)]
    [string]$ServerName = "(localdb)\MSSQLLocalDB",
    
    [Parameter(Mandatory=$false)]
    [string]$DatabaseName = "CabarlesIPT102",
    
    [Parameter(Mandatory=$false)]
    [string]$Username = "",
    
    [Parameter(Mandatory=$false)]
    [string]$Password = ""
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Database Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Server: $ServerName" -ForegroundColor Yellow
Write-Host "Database: $DatabaseName" -ForegroundColor Yellow
Write-Host ""

# Build connection string for sqlcmd
if ($Username -ne "" -and $Password -ne "") {
    $connectionArgs = "-S `"$ServerName`" -U `"$Username`" -P `"$Password`""
    $appConnectionString = "Server=$ServerName;Database=$DatabaseName;User Id=$Username;Password=$Password;TrustServerCertificate=True;"
    Write-Host "Authentication: SQL Server Authentication" -ForegroundColor Yellow
} else {
    $connectionArgs = "-S `"$ServerName`" -E"
    $appConnectionString = "Server=$ServerName;Database=$DatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
    Write-Host "Authentication: Windows Authentication" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Creating database..." -ForegroundColor Green

# Create temporary SQL script with dynamic database name
$sqlScript = @"
-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = '$DatabaseName')
BEGIN
    ALTER DATABASE [$DatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$DatabaseName];
END
GO

-- Create database
CREATE DATABASE [$DatabaseName];
GO

USE [$DatabaseName];
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

PRINT 'Database $DatabaseName created successfully with 3 sample students!';
GO
"@

# Save to temp file
$tempSqlFile = "temp_setup.sql"
$sqlScript | Out-File -FilePath $tempSqlFile -Encoding UTF8

# Execute SQL script
try {
    $command = "sqlcmd $connectionArgs -i `"$tempSqlFile`""
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0 -or $LASTEXITCODE -eq -1) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "  Database created successfully!" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
        Write-Host ""
        
        # Update appsettings.json
        Write-Host "Updating appsettings.json..." -ForegroundColor Yellow
        
        $appsettingsPath = "CabarlesWPF\appsettings.json"
        $appsettingsContent = @{
            "ConnectionStrings" = @{
                "DefaultConnection" = $appConnectionString
            }
        }
        
        $appsettingsContent | ConvertTo-Json | Set-Content -Path $appsettingsPath
        
        Write-Host "appsettings.json updated!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Connection String:" -ForegroundColor Cyan
        Write-Host $appConnectionString -ForegroundColor White
        Write-Host ""
    } else {
        Write-Host "Error creating database. Exit code: $LASTEXITCODE" -ForegroundColor Red
    }
} catch {
    Write-Host "Error: $_" -ForegroundColor Red
} finally {
    # Clean up temp file
    if (Test-Path $tempSqlFile) {
        Remove-Item $tempSqlFile
    }
}

Write-Host ""
Write-Host "Done!" -ForegroundColor Cyan
