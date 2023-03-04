CREATE TABLE [dbo].[GroupStatus] (
  [Id] [int] IDENTITY,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  CONSTRAINT [PK_GroupStatus_Id] PRIMARY KEY CLUSTERED ([Id])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO