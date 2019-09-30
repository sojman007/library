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

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Books]') AND [c].[name] = N'Title');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Books] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Books] ALTER COLUMN [Title] nvarchar(450) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Books]') AND [c].[name] = N'ISBN');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Books] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Books] ALTER COLUMN [ISBN] nvarchar(450) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Books]') AND [c].[name] = N'Author');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Books] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Books] ALTER COLUMN [Author] nvarchar(450) NULL;

GO

CREATE INDEX [IX_Books_Author] ON [Books] ([Author]);

GO

CREATE UNIQUE INDEX [IX_Books_ISBN] ON [Books] ([ISBN]) WHERE [ISBN] IS NOT NULL;

GO

CREATE INDEX [IX_Books_Title] ON [Books] ([Title]);

GO

CREATE INDEX [IX_BookHistories_BookId] ON [BookHistories] ([BookId]);

GO

CREATE INDEX [IX_BookHistories_LenderId] ON [BookHistories] ([LenderId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190930141140_indexes', N'2.2.4-servicing-10062');

GO

