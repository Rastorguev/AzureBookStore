CREATE TABLE [dbo].[Players] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NULL,
    [Age]      INT            NOT NULL,
    [Position] NVARCHAR (MAX) NULL,
    [TeamId]   INT            NULL,
    CONSTRAINT [PK_dbo.Players] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Players_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TeamId]
    ON [dbo].[Players]([TeamId] ASC);

