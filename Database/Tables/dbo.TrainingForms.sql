CREATE TABLE [dbo].[TrainingForms] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_TrainingForms_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO