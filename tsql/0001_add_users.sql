CREATE LOGIN [bookadmin] WITH PASSWORD = 'bookadmin'
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'bookadmin')
BEGIN
    CREATE USER [bookadmin] FOR LOGIN [bookadmin]
    EXEC sp_addrolemember N'db_owner', N'bookadmin'
END;
GO