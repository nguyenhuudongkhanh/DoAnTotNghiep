CREATE TABLE [dbo].[ThesisGroupDetails] (
  [StudentThesisGroupId] [varchar](50) NOT NULL,
  [StudentId] [varchar](50) NOT NULL,
  [Notes] [ntext] NULL,
  [IsLeader] [bit] NOT NULL,
  [StatusId] [int] NOT NULL DEFAULT (1),
  [SupervisorPoint] [decimal](5, 2) NULL,
  [CtrArgPoint] [decimal](5, 2) NULL,
  [CommitteePoint] [decimal](5, 2) NULL,
  [FinalPoint] [decimal](5, 2) NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([StudentThesisGroupId], [StudentId]),
  CHECK ([CommitteePoint]>=(0)),
  CHECK ([CtrArgPoint]>=(0)),
  CHECK ([SupervisorPoint]>=(0))
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ThesisGroupDetails]
  ADD CONSTRAINT [FK_ThesisGroupDetails_GroupStatus_Id] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GroupStatus] ([Id])
GO

ALTER TABLE [dbo].[ThesisGroupDetails]
  ADD CONSTRAINT [FK_ThesisGroupDetails_Students_ID] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ThesisGroupDetails]
  ADD CONSTRAINT [FK_ThesisGroupDetails_ThesisGroups_ID] FOREIGN KEY ([StudentThesisGroupId]) REFERENCES [dbo].[ThesisGroups] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO