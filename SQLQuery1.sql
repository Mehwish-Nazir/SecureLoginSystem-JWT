Create database AuthenticationDb;
use AuthenticationDb;
CREATE TABLE Users (
    UserID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(15) NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    IsEmailConfirmed BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginAt DATETIME NULL,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LockOutEnd DATETIME NULL,
    ProfileImagePath NVARCHAR(500) NULL,
    EmailConfirmationToken NVARCHAR(100) NULL,
    EmailConfirmationExpiry DATETIME NULL,
    RoleId INT NOT NULL DEFAULT 2  -- Default to 'Customer'
);

ALTER TABLE Users
ADD RoleId INT NOT NULL DEFAULT 2;  -- set default role assuming 2 is 'Customer' role

--Add FK contraint in Users table , first I have creatdRoleId Cloumn and now I am creating Contraint
ALTER TABLE Users
ADD CONSTRAINT FK_Users_Roles_RoleId
FOREIGN KEY (RoleId) REFERENCES Roles(RoleId);

use AuthenticationDb;
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(100) NOT NULL
);

INSERT INTO AuthenticationDb.dbo.Roles (RoleName) VALUES ('Admin'), ('Customer'), ('Employee');


ALTER TABLE AuthenticationDb.dbo.Users
ADD EmailConfirmationToken NVARCHAR(100) NULL,
    EmailConfirmationExpiry DATETIME NULL;

select *from AuthenticationDb.dbo.Roles;
select *from AuthenticationDb.dbo.Users;
ALTER TABLE AuthenticationDb.dbo.Users DROP COLUMN Useranme;
EXEC sp_rename 'AuthenticationDb.dbo.Users.Useranme', 'Username', 'COLUMN';
delete from AuthenticationDb.dbo.Users where Email='AqKhan@gmail.com';
ALTER TABLE AuthenticationDb.dbo.Users DROP CONSTRAINT UQ__Users__FD039823AC3B8662;
ALTER TABLE AuthenticationDb.dbo.Users DROP COLUMN Useranme;
ALTER TABLE AuthenticationDb.dbo.Users ADD Username nvarchar(100) NOT NULL UNIQUE;
SELECT Email, PasswordHash, RoleId FROM AuthenticationDb.dbo.Users WHERE Email = 'nazir@gmail.com';
ALTER TABLE AuthenticationDb.dbo.Users
DROP COLUMN PasswordSalt;
SELECT * FROM AuthenticationDb.dbo.Roles WHERE RoleId = 2;

