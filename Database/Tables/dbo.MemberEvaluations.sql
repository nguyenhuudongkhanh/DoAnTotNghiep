CREATE TABLE [dbo].[MemberEvaluations] (
  [CommitteeMemberResultId] [varchar](50) NOT NULL,
  [EvalutionPatternId] [varchar](50) NOT NULL,
  [Name] [nvarchar](450) NOT NULL,
  [Point] [decimal] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_Evaluations_CommitteeMemberId] PRIMARY KEY CLUSTERED ([CommitteeMemberResultId], [EvalutionPatternId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[MemberEvaluations]
  ADD CONSTRAINT [FK_MemberEvaluations_MemberEvalutionPatterns_ID] FOREIGN KEY ([EvalutionPatternId]) REFERENCES [dbo].[MemberEvalutionPatterns] ([ID])
GO