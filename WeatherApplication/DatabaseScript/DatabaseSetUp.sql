USE master
GO

CREATE DATABASE WeatherApp
GO

USE WeatherApp
GO

CREATE TABLE Users(
[Id] [int] IDENTITY (1,1) NOT NULL,
[RefreshToken] [varchar](200),
[ClientId] [varchar](200)
CONSTRAINT [PK_UsersId] PRIMARY KEY CLUSTERED ([id] ASC),
);

CREATE TABLE Locations (
[Id] [int] IDENTITY (1,1) NOT NULL,
[Location] [varchar](255),
CONSTRAINT [PK_LocationsId] PRIMARY KEY CLUSTERED ([id] ASC),
);
GO

CREATE TABLE UserLocations (
[Id] [int] IDENTITY (1,1) PRIMARY KEY NOT NULL,
[UserId] [int] NOT NULL,
[LocationId] [int] NOT NULL,
FOREIGN KEY ([UserID]) REFERENCES Users(Id),
FOREIGN KEY ([LocationId]) REFERENCES Locations(Id),
);
GO

SELECT * FROM UserLocations;

USE master
DROP DATABASE WeatherApp