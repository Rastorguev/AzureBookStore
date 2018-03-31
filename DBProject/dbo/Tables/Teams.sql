CREATE TABLE [dbo].[Teams] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (MAX) NULL,
    [Coach] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC)
);

