CREATE TABLE [dbo].[CounterArgumentResults] (
  [ThesisId] [varchar](50) NOT NULL,
  [LecturerId] [varchar](50) NOT NULL,
  [Contents] [ntext] NULL,
  [ResearchMethods] [ntext] NULL,
  [ScientificResults] [ntext] NULL,
  [PracticalResults] [ntext] NULL,
  [Defects] [ntext] NULL,
  [Conclusions] [ntext] NULL,
  [Answers] [ntext] NULL,
  [Point] [decimal](5, 2) NULL,
  [IsCompleted] [bit] NOT NULL,
  CONSTRAINT [PK_Counterargument_PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ThesisId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CounterArgumentResults]
  ADD CONSTRAINT [FK_Counterargument_FacultyStaffs_ID] FOREIGN KEY ([LecturerId]) REFERENCES [dbo].[FacultyStaffs] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[CounterArgumentResults]
  ADD CONSTRAINT [FK_CounterArguments_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO