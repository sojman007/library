create database library;

use library;

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Tokens] (
    [Id] bigint NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NULL,
    [UserId] bigint NOT NULL,
    [Token] nvarchar(max) NULL,
    CONSTRAINT [PK_Tokens] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NULL,
    [Name] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Salt] nvarchar(max) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [IsAdmin] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190930074627_init', N'2.2.4-servicing-10062');

GO

