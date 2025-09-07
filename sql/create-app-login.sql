-- Creates a dedicated SQL login for the application with minimal permissions.
-- Usage:
--   sqlcmd -S <server> -U sa -P <sa_password> -i create-app-login.sql -v APP_DB_PASSWORD="StrongPassword!1"

IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'csapp')
BEGIN
    CREATE LOGIN csapp WITH PASSWORD = '$(APP_DB_PASSWORD)';
END
GO
USE csproject;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'csapp')
BEGIN
    CREATE USER csapp FOR LOGIN csapp;
    ALTER ROLE db_datareader ADD MEMBER csapp;
    ALTER ROLE db_datawriter ADD MEMBER csapp;
END
GO
