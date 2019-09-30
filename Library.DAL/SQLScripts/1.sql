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

CREATE TABLE [BookHistories] (
    [Id] bigint NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NULL,
    [BookId] bigint NOT NULL,
    [LenderId] bigint NOT NULL,
    [Returned] bit NOT NULL,
    CONSTRAINT [PK_BookHistories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Books] (
    [Id] bigint NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NULL,
    [Title] nvarchar(max) NULL,
    [Author] nvarchar(max) NULL,
    [ISBN] nvarchar(max) NULL,
    [IsAvailable] bit NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190930074705_init', N'2.2.4-servicing-10062');

GO

