CREATE TABLE [dbo].[TrainingLevels] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_TrainingLevel_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO