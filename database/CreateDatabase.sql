IF DB_ID('OdbcSeminarski') IS NULL
BEGIN
    CREATE DATABASE OdbcSeminarski;
END
GO

USE OdbcSeminarski;
GO

IF OBJECT_ID('dbo.Students', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(80) NOT NULL,
        LastName NVARCHAR(80) NOT NULL,
        IndexNumber NVARCHAR(30) NOT NULL UNIQUE,
        Email NVARCHAR(120) NOT NULL,
        StudyYear INT NOT NULL CHECK (StudyYear BETWEEN 1 AND 6)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Students)
BEGIN
    INSERT INTO dbo.Students (FirstName, LastName, IndexNumber, Email, StudyYear) VALUES
    (N'Nikola', N'Nikolić', N'IT 12/23', N'nikola.nikolic@example.com', 3),
    (N'Milica', N'Jovanović', N'IT 27/24', N'milica.jovanovic@example.com', 2),
    (N'Stefan', N'Petrović', N'IT 05/22', N'stefan.petrovic@example.com', 4);
END
GO
