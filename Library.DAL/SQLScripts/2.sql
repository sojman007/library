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

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PasswordHash');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] ALTER COLUMN [PasswordHash] nvarchar(450) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(450) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tokens]') AND [c].[name] = N'Token');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Tokens] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Tokens] ALTER COLUMN [Token] nvarchar(450) NULL;

GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]) WHERE [Email] IS NOT NULL;

GO

CREATE INDEX [IX_Users_PasswordHash] ON [Users] ([PasswordHash]);

GO

CREATE INDEX [IX_Tokens_Token] ON [Tokens] ([Token]);

GO

CREATE INDEX [IX_Tokens_UserId] ON [Tokens] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190930141217_indexes', N'2.2.4-servicing-10062');

GO

