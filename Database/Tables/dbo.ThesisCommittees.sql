CREATE TABLE [dbo].[ThesisCommittees] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [CouncilId] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Council_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ThesisCommittees]
  ADD CONSTRAINT [FK_ThesisCommittees_Councils_ID] FOREIGN KEY ([CouncilId]) REFERENCES [dbo].[Councils] ([ID]) ON DELETE CASCADE
GO