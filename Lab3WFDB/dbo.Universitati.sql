CREATE TABLE [dbo].[Universitati] (
    [Id]       INT  NOT NULL,
    [NameUniv] VARCHAR(50) NOT NULL,
    [City]     VARCHAR(50) NOT NULL,
    [Code]     INT  NOT NULL,
    CONSTRAINT [PK_Universitati] PRIMARY KEY CLUSTERED ([Code] ASC)
);

