
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/06/2014 20:06:30
-- Generated from EDMX file: C:\Users\mamar_000\Desktop\authink-web\authink-web\Authink.Data\AuthinkData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Authink.Admin.v2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PictureColor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Colors] DROP CONSTRAINT [FK_PictureColor];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_UserTask];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Child_Test_Children]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Child_Test] DROP CONSTRAINT [FK_AS_Child_Test_Children];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Child_Test_Tests]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Child_Test] DROP CONSTRAINT [FK_AS_Child_Test_Tests];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Picture_Task_Pictures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Picture_Task] DROP CONSTRAINT [FK_AS_Picture_Task_Pictures];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Picture_Task_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Picture_Task] DROP CONSTRAINT [FK_AS_Picture_Task_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Test_Task_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Test_Task] DROP CONSTRAINT [FK_AS_Test_Task_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_AS_Test_Task_Tests]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AS_Test_Task] DROP CONSTRAINT [FK_AS_Test_Task_Tests];
GO
IF OBJECT_ID(N'[dbo].[FK_UserChild_Children]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserChild] DROP CONSTRAINT [FK_UserChild_Children];
GO
IF OBJECT_ID(N'[dbo].[FK_UserChild_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserChild] DROP CONSTRAINT [FK_UserChild_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tests] DROP CONSTRAINT [FK_UserTest];
GO
IF OBJECT_ID(N'[dbo].[FK_SoundTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_SoundTask];
GO
IF OBJECT_ID(N'[dbo].[FK_SoundPicture]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pictures] DROP CONSTRAINT [FK_SoundPicture];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPasswordResetToken]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PasswordResetTokens] DROP CONSTRAINT [FK_UserPasswordResetToken];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Children]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Children];
GO
IF OBJECT_ID(N'[dbo].[Colors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Colors];
GO
IF OBJECT_ID(N'[dbo].[Pictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pictures];
GO
IF OBJECT_ID(N'[dbo].[Sounds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sounds];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[Tests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tests];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[PasswordResetTokens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PasswordResetTokens];
GO
IF OBJECT_ID(N'[dbo].[AS_Child_Test]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AS_Child_Test];
GO
IF OBJECT_ID(N'[dbo].[AS_Picture_Task]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AS_Picture_Task];
GO
IF OBJECT_ID(N'[dbo].[AS_Test_Task]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AS_Test_Task];
GO
IF OBJECT_ID(N'[dbo].[UserChild]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserChild];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Children'
CREATE TABLE [dbo].[Children] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DateOfBirth] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [Center] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [DescriptionOfCondition] nvarchar(max)  NULL,
    [PlaceOfBirth] nvarchar(max)  NULL,
    [ParentName] nvarchar(max)  NULL,
    [IsHidden] bit  NOT NULL,
    [ProfilePictureUrl] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Colors'
CREATE TABLE [dbo].[Colors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [PictureId] int  NOT NULL,
    [IsCorrect] bit  NOT NULL
);
GO

-- Creating table 'Pictures'
CREATE TABLE [dbo].[Pictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [IsHidden] bit  NOT NULL,
    [Theme] nvarchar(max)  NULL,
    [IsAnswer] bit  NULL,
    [Sound_Id] int  NULL
);
GO

-- Creating table 'Sounds'
CREATE TABLE [dbo].[Sounds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [IsHidden] bit  NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [IsHidden] bit  NOT NULL,
    [Difficulty] int  NOT NULL,
    [ProfilePictureUrl] nvarchar(max)  NOT NULL,
    [Sound_Id] int  NULL
);
GO

-- Creating table 'Tests'
CREATE TABLE [dbo].[Tests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ShortDescription] nvarchar(max)  NOT NULL,
    [LongDescription] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [IsHidden] bit  NOT NULL
);
GO

-- Creating table 'PasswordResetTokens'
CREATE TABLE [dbo].[PasswordResetTokens] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [UsedOn] datetime  NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'AS_Child_Test'
CREATE TABLE [dbo].[AS_Child_Test] (
    [Children_Id] int  NOT NULL,
    [Tests_Id] int  NOT NULL
);
GO

-- Creating table 'AS_Picture_Task'
CREATE TABLE [dbo].[AS_Picture_Task] (
    [Pictures_Id] int  NOT NULL,
    [Tasks_Id] int  NOT NULL
);
GO

-- Creating table 'AS_Test_Task'
CREATE TABLE [dbo].[AS_Test_Task] (
    [Tasks_Id] int  NOT NULL,
    [Tests_Id] int  NOT NULL
);
GO

-- Creating table 'UserChild'
CREATE TABLE [dbo].[UserChild] (
    [Children_Id] int  NOT NULL,
    [Users_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Children'
ALTER TABLE [dbo].[Children]
ADD CONSTRAINT [PK_Children]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [PK_Colors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [PK_Pictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sounds'
ALTER TABLE [dbo].[Sounds]
ADD CONSTRAINT [PK_Sounds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [PK_Tests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PasswordResetTokens'
ALTER TABLE [dbo].[PasswordResetTokens]
ADD CONSTRAINT [PK_PasswordResetTokens]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Children_Id], [Tests_Id] in table 'AS_Child_Test'
ALTER TABLE [dbo].[AS_Child_Test]
ADD CONSTRAINT [PK_AS_Child_Test]
    PRIMARY KEY CLUSTERED ([Children_Id], [Tests_Id] ASC);
GO

-- Creating primary key on [Pictures_Id], [Tasks_Id] in table 'AS_Picture_Task'
ALTER TABLE [dbo].[AS_Picture_Task]
ADD CONSTRAINT [PK_AS_Picture_Task]
    PRIMARY KEY CLUSTERED ([Pictures_Id], [Tasks_Id] ASC);
GO

-- Creating primary key on [Tasks_Id], [Tests_Id] in table 'AS_Test_Task'
ALTER TABLE [dbo].[AS_Test_Task]
ADD CONSTRAINT [PK_AS_Test_Task]
    PRIMARY KEY CLUSTERED ([Tasks_Id], [Tests_Id] ASC);
GO

-- Creating primary key on [Children_Id], [Users_Id] in table 'UserChild'
ALTER TABLE [dbo].[UserChild]
ADD CONSTRAINT [PK_UserChild]
    PRIMARY KEY CLUSTERED ([Children_Id], [Users_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PictureId] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [FK_PictureColor]
    FOREIGN KEY ([PictureId])
    REFERENCES [dbo].[Pictures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PictureColor'
CREATE INDEX [IX_FK_PictureColor]
ON [dbo].[Colors]
    ([PictureId]);
GO

-- Creating foreign key on [UserId] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_UserTask]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTask'
CREATE INDEX [IX_FK_UserTask]
ON [dbo].[Tasks]
    ([UserId]);
GO

-- Creating foreign key on [Children_Id] in table 'AS_Child_Test'
ALTER TABLE [dbo].[AS_Child_Test]
ADD CONSTRAINT [FK_AS_Child_Test_Children]
    FOREIGN KEY ([Children_Id])
    REFERENCES [dbo].[Children]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tests_Id] in table 'AS_Child_Test'
ALTER TABLE [dbo].[AS_Child_Test]
ADD CONSTRAINT [FK_AS_Child_Test_Tests]
    FOREIGN KEY ([Tests_Id])
    REFERENCES [dbo].[Tests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AS_Child_Test_Tests'
CREATE INDEX [IX_FK_AS_Child_Test_Tests]
ON [dbo].[AS_Child_Test]
    ([Tests_Id]);
GO

-- Creating foreign key on [Pictures_Id] in table 'AS_Picture_Task'
ALTER TABLE [dbo].[AS_Picture_Task]
ADD CONSTRAINT [FK_AS_Picture_Task_Pictures]
    FOREIGN KEY ([Pictures_Id])
    REFERENCES [dbo].[Pictures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tasks_Id] in table 'AS_Picture_Task'
ALTER TABLE [dbo].[AS_Picture_Task]
ADD CONSTRAINT [FK_AS_Picture_Task_Tasks]
    FOREIGN KEY ([Tasks_Id])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AS_Picture_Task_Tasks'
CREATE INDEX [IX_FK_AS_Picture_Task_Tasks]
ON [dbo].[AS_Picture_Task]
    ([Tasks_Id]);
GO

-- Creating foreign key on [Tasks_Id] in table 'AS_Test_Task'
ALTER TABLE [dbo].[AS_Test_Task]
ADD CONSTRAINT [FK_AS_Test_Task_Tasks]
    FOREIGN KEY ([Tasks_Id])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tests_Id] in table 'AS_Test_Task'
ALTER TABLE [dbo].[AS_Test_Task]
ADD CONSTRAINT [FK_AS_Test_Task_Tests]
    FOREIGN KEY ([Tests_Id])
    REFERENCES [dbo].[Tests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AS_Test_Task_Tests'
CREATE INDEX [IX_FK_AS_Test_Task_Tests]
ON [dbo].[AS_Test_Task]
    ([Tests_Id]);
GO

-- Creating foreign key on [Children_Id] in table 'UserChild'
ALTER TABLE [dbo].[UserChild]
ADD CONSTRAINT [FK_UserChild_Children]
    FOREIGN KEY ([Children_Id])
    REFERENCES [dbo].[Children]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_Id] in table 'UserChild'
ALTER TABLE [dbo].[UserChild]
ADD CONSTRAINT [FK_UserChild_Users]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserChild_Users'
CREATE INDEX [IX_FK_UserChild_Users]
ON [dbo].[UserChild]
    ([Users_Id]);
GO

-- Creating foreign key on [UserId] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [FK_UserTest]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTest'
CREATE INDEX [IX_FK_UserTest]
ON [dbo].[Tests]
    ([UserId]);
GO

-- Creating foreign key on [Sound_Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_SoundTask]
    FOREIGN KEY ([Sound_Id])
    REFERENCES [dbo].[Sounds]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SoundTask'
CREATE INDEX [IX_FK_SoundTask]
ON [dbo].[Tasks]
    ([Sound_Id]);
GO

-- Creating foreign key on [Sound_Id] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [FK_SoundPicture]
    FOREIGN KEY ([Sound_Id])
    REFERENCES [dbo].[Sounds]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SoundPicture'
CREATE INDEX [IX_FK_SoundPicture]
ON [dbo].[Pictures]
    ([Sound_Id]);
GO

-- Creating foreign key on [UserId] in table 'PasswordResetTokens'
ALTER TABLE [dbo].[PasswordResetTokens]
ADD CONSTRAINT [FK_UserPasswordResetToken]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPasswordResetToken'
CREATE INDEX [IX_FK_UserPasswordResetToken]
ON [dbo].[PasswordResetTokens]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------