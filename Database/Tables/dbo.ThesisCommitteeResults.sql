CREATE TABLE [dbo].[ThesisCommitteeResults] (
  [ThesisId] [varchar](50) NOT NULL,
  [ThesisCommitteeId] [varchar](50) NOT NULL,
  [Contents] [ntext] NULL,
  [Conclusions] [ntext] NULL,
  [Point] [decimal](5, 2) NULL,
  CONSTRAINT [PK_ThesisCommitteeResults_ThesisId] PRIMARY KEY CLUSTERED ([ThesisId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ThesisCommitteeResults]
  ADD CONSTRAINT [FK_ThesisCommitteeResults_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ThesisCommitteeResults]
  ADD CONSTRAINT [FK_ThesisCommitteeResults_ThesisCommittees_ID] FOREIGN KEY ([ThesisCommitteeId]) REFERENCES [dbo].[ThesisCommittees] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO