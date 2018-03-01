
SELECT * FROM auth_User 
IF OBJECT_ID ('dbo.auth_User') IS NOT NULL
	DROP TABLE dbo.auth_User
GO

CREATE TABLE dbo.auth_User
	(
	Id                     BIGINT IDENTITY NOT NULL,
	TenantId               INT,
	Name                   VARCHAR (50),
	Password               VARCHAR (255),
	SecurityStamp          VARCHAR (100),
	FullName               VARCHAR (50),
	Surname                VARCHAR (50),
	PhoneNumber            VARCHAR (50),
	IsPhoneNumberConfirmed BIT DEFAULT ((0)) NOT NULL,
	EmailAddress           VARCHAR (50),
	IsEmailConfirmed       BIT DEFAULT ((0)) NOT NULL,
	EmailConfirmationCode  VARCHAR (50),
	IsActive               TINYINT DEFAULT ((0)) NOT NULL,
	PasswordResetCode      VARCHAR (10),
	LastLoginTime          DATETIME,
	IsLockoutEnabled       BIT DEFAULT ((0)) NOT NULL,
	AccessFailedCount      VARCHAR (50),
	LockoutEndDateUtc      DATETIME,
	PRIMARY KEY (Id)
	)
GO

