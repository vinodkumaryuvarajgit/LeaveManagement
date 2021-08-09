-- create database [LMSDb]
IF DB_ID(N'LMSDb') IS NOT NULL
BEGIN
	USE [master]
	DROP DATABASE [LMSDb]
END
GO

CREATE DATABASE [LMSDb]
GO

USE [LMSDb]
GO

-- create table scripts
IF OBJECT_ID(N'dbo.Gender', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Gender
END
GO
CREATE TABLE dbo.Gender
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(10),
	CHECK([Name] in ('Male','Female','Unknown'))
)
GO

IF OBJECT_ID(N'dbo.AccessType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.AccessType
END
GO
CREATE TABLE dbo.AccessType
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(10),
	CHECK([Name] in ('Employee','Admin'))
)
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Users
END
GO
CREATE TABLE dbo.Users
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	GenderId INT NOT NULL FOREIGN KEY REFERENCES Gender(Id),
	Email VARCHAR(50) NOT NULL,
	PasswordHash VARCHAR(100) NOT NULL,
	AccessTypeId INT NOT NULL FOREIGN KEY REFERENCES AccessType(Id),
	IsActive BIT DEFAULT 1,
	CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
	PaternityLeave INT,
	MaternityLeave INT,
	SickLeave INT,
	EarnedLeave INT,
	CasualLeave INT
)
GO

IF OBJECT_ID(N'dbo.LeaveType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.LeaveType
END
GO
CREATE TABLE dbo.LeaveType
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(2),
	[Description] VARCHAR(20),
	DefaultDays INT
)
GO

IF OBJECT_ID(N'dbo.ApprovalStatus', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.ApprovalStatus
END
GO
CREATE TABLE dbo.ApprovalStatus
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(10),
	CHECK([Name] in ('Pending','Approved','Rejected'))
)
GO

IF OBJECT_ID(N'dbo.LeaveRequest', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.LeaveRequest
END
GO
CREATE TABLE dbo.LeaveRequest
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
	LeaveTypeId INT NOT NULL FOREIGN KEY REFERENCES LeaveType(Id),
	FromDate DATETIME NOT NULL,
	ToDate DATETIME NOT NULL,
	TotalDays INT NOT NULL,
	Reason VARCHAR(100),
	DocumentName VARCHAR(50),
	CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
	ApprovalStatusId INT FOREIGN KEY REFERENCES ApprovalStatus(Id),
	ApprovedOn DATETIME,
	ApprovedBy INT FOREIGN KEY REFERENCES Users(Id)
)
GO

-- insert scripts
INSERT INTO Gender([Name]) VALUES('Male')
INSERT INTO Gender([Name]) VALUES('Female')
INSERT INTO Gender([Name]) VALUES('Unknown')
GO

INSERT INTO AccessType([Name]) VALUES('Admin')
INSERT INTO AccessType([Name]) VALUES('Employee')
GO

INSERT INTO Users(FirstName,LastName,GenderId,Email,PasswordHash,AccessTypeId,CreatedBy) 
	VALUES('Janarthanan','D',1,'jana.d@gmail.com','TQskzK3iLfbRVHeM1muvBCiKribfl6lh8+o91hb74G3OvsybvkzpPI4S3KIeWTXAiwlUU0iSxWi4wSuS8mokSA==',1,1) -- Admin123
INSERT INTO Users(FirstName,LastName,GenderId,Email,PasswordHash,AccessTypeId,CreatedBy) 
	VALUES('Vinod Kumar','Y',1,'vinod.y@gmail.com','uHf9gU/1YIfbj9N9F8Xt1b5G6XTfo2ptBJN0ABJX6oIydPwwrqBw0IFbpYNrjUnVBpJQpM6oGH1XeeIVkJSzyw==',2,1) -- Employee123
INSERT INTO Users(FirstName,LastName,GenderId,Email,PasswordHash,AccessTypeId,CreatedBy) 
	VALUES('Arjun','S',1,'arjun.s@gmail.com','uHf9gU/1YIfbj9N9F8Xt1b5G6XTfo2ptBJN0ABJX6oIydPwwrqBw0IFbpYNrjUnVBpJQpM6oGH1XeeIVkJSzyw==',2,1) -- Employee123
GO

INSERT INTO ApprovalStatus([Name]) VALUES('Pending')
INSERT INTO ApprovalStatus([Name]) VALUES('Approved')
INSERT INTO ApprovalStatus([Name]) VALUES('Rejected')
GO

Insert into LeaveType values ('EL','Earned Leave',12)
Insert into LeaveType values ('SL','Sick Leave',12)
Insert into LeaveType values ('CL','Casual Leave',6)
Insert into LeaveType values ('PL','Paternity Leave',15)
Insert into LeaveType values ('ML','Maternity Leave',90)
GO

-- create procedure scripts
IF OBJECT_ID(N'dbo.UpdateCountSL', N'P') IS NOT NULL
BEGIN
Print 'Hi'
	DROP PROCEDURE dbo.UpdateCountSL
END
GO
-- This procedure to be scheduled as job to update the sick leave count monthly
CREATE PROCEDURE dbo.UpdateCountSL
AS
BEGIN
	IF (GETDATE() = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0))
    BEGIN
        UPDATE LeaveType SET [DefaultDays] = [DefaultDays] + 2 WHERE [Name] = 'SL'
    END
END
GO
