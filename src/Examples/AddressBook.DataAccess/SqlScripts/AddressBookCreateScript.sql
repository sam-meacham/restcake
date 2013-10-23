-- Create a database named AddressBook_RestCake, and then run this script to create all of the tables
-- Then you can run the data script to populate them.

-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/13/2011 14:54:50
-- Generated from EDMX file: C:\projects\3rdParty\RestCake\src\AddressBook.DataAccess\AddressBookEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AddressBook_RestCake];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Address_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Email_EmailType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Email] DROP CONSTRAINT [FK_Email_EmailType];
GO
IF OBJECT_ID(N'[dbo].[FK_Email_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Email] DROP CONSTRAINT [FK_Email_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonGroup_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonGroup] DROP CONSTRAINT [FK_PersonGroup_Group];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonGroup_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonGroup] DROP CONSTRAINT [FK_PersonGroup_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Phone_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Phone] DROP CONSTRAINT [FK_Phone_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Phone_PhoneType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Phone] DROP CONSTRAINT [FK_Phone_PhoneType];
GO
IF OBJECT_ID(N'[dbo].[FK_Website_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Website] DROP CONSTRAINT [FK_Website_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Email]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Email];
GO
IF OBJECT_ID(N'[dbo].[EmailType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailType];
GO
IF OBJECT_ID(N'[dbo].[Group]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[PersonGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonGroup];
GO
IF OBJECT_ID(N'[dbo].[Phone]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Phone];
GO
IF OBJECT_ID(N'[dbo].[PhoneType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhoneType];
GO
IF OBJECT_ID(N'[dbo].[Website]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Website];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EmailAddresses'
CREATE TABLE [dbo].[EmailAddresses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PersonID] int  NOT NULL,
    [Email] varchar(320)  NOT NULL,
    [EmailTypeID] int  NOT NULL
);
GO

-- Creating table 'EmailTypes'
CREATE TABLE [dbo].[EmailTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Description] varchar(50)  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Fname] varchar(50)  NULL,
    [Lname] varchar(50)  NULL,
    [Title] varchar(50)  NULL,
    [Company] varchar(50)  NULL,
    [Birthday] datetime  NULL,
    [Notes] varchar(2000)  NULL,
    [DateCreated] datetime  NOT NULL,
    [DateModified] datetime  NOT NULL
);
GO

-- Creating table 'Phones'
CREATE TABLE [dbo].[Phones] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PersonID] int  NOT NULL,
    [PhoneNumber] varchar(50)  NOT NULL,
    [PhoneTypeID] int  NOT NULL
);
GO

-- Creating table 'PhoneTypes'
CREATE TABLE [dbo].[PhoneTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Description] varchar(50)  NOT NULL
);
GO

-- Creating table 'Websites'
CREATE TABLE [dbo].[Websites] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PersonID] int  NOT NULL,
    [Url] varchar(2000)  NOT NULL,
    [Description] varchar(500)  NULL
);
GO

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PersonID] int  NOT NULL,
    [Address1] varchar(100)  NULL,
    [Address2] varchar(100)  NULL,
    [City] varchar(100)  NULL,
    [State] varchar(100)  NULL,
    [Zip] varchar(50)  NULL,
    [DateCreated] datetime  NOT NULL,
    [DateModified] datetime  NOT NULL
);
GO

-- Creating table 'PersonGroup'
CREATE TABLE [dbo].[PersonGroup] (
    [Groups_ID] int  NOT NULL,
    [People_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'EmailAddresses'
ALTER TABLE [dbo].[EmailAddresses]
ADD CONSTRAINT [PK_EmailAddresses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EmailTypes'
ALTER TABLE [dbo].[EmailTypes]
ADD CONSTRAINT [PK_EmailTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Phones'
ALTER TABLE [dbo].[Phones]
ADD CONSTRAINT [PK_Phones]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PhoneTypes'
ALTER TABLE [dbo].[PhoneTypes]
ADD CONSTRAINT [PK_PhoneTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Websites'
ALTER TABLE [dbo].[Websites]
ADD CONSTRAINT [PK_Websites]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Groups_ID], [People_ID] in table 'PersonGroup'
ALTER TABLE [dbo].[PersonGroup]
ADD CONSTRAINT [PK_PersonGroup]
    PRIMARY KEY NONCLUSTERED ([Groups_ID], [People_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EmailTypeID] in table 'EmailAddresses'
ALTER TABLE [dbo].[EmailAddresses]
ADD CONSTRAINT [FK_Email_EmailType]
    FOREIGN KEY ([EmailTypeID])
    REFERENCES [dbo].[EmailTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Email_EmailType'
CREATE INDEX [IX_FK_Email_EmailType]
ON [dbo].[EmailAddresses]
    ([EmailTypeID]);
GO

-- Creating foreign key on [PersonID] in table 'EmailAddresses'
ALTER TABLE [dbo].[EmailAddresses]
ADD CONSTRAINT [FK_Email_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[People]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Email_Person'
CREATE INDEX [IX_FK_Email_Person]
ON [dbo].[EmailAddresses]
    ([PersonID]);
GO

-- Creating foreign key on [PersonID] in table 'Phones'
ALTER TABLE [dbo].[Phones]
ADD CONSTRAINT [FK_Phone_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[People]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Phone_Person'
CREATE INDEX [IX_FK_Phone_Person]
ON [dbo].[Phones]
    ([PersonID]);
GO

-- Creating foreign key on [PersonID] in table 'Websites'
ALTER TABLE [dbo].[Websites]
ADD CONSTRAINT [FK_Website_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[People]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Website_Person'
CREATE INDEX [IX_FK_Website_Person]
ON [dbo].[Websites]
    ([PersonID]);
GO

-- Creating foreign key on [PhoneTypeID] in table 'Phones'
ALTER TABLE [dbo].[Phones]
ADD CONSTRAINT [FK_Phone_PhoneType]
    FOREIGN KEY ([PhoneTypeID])
    REFERENCES [dbo].[PhoneTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Phone_PhoneType'
CREATE INDEX [IX_FK_Phone_PhoneType]
ON [dbo].[Phones]
    ([PhoneTypeID]);
GO

-- Creating foreign key on [Groups_ID] in table 'PersonGroup'
ALTER TABLE [dbo].[PersonGroup]
ADD CONSTRAINT [FK_PersonGroup_Group]
    FOREIGN KEY ([Groups_ID])
    REFERENCES [dbo].[Groups]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [People_ID] in table 'PersonGroup'
ALTER TABLE [dbo].[PersonGroup]
ADD CONSTRAINT [FK_PersonGroup_Person]
    FOREIGN KEY ([People_ID])
    REFERENCES [dbo].[People]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonGroup_Person'
CREATE INDEX [IX_FK_PersonGroup_Person]
ON [dbo].[PersonGroup]
    ([People_ID]);
GO

-- Creating foreign key on [ID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_Address_Address]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Addresses]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PersonID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_Address_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[People]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Address_Person'
CREATE INDEX [IX_FK_Address_Person]
ON [dbo].[Addresses]
    ([PersonID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

