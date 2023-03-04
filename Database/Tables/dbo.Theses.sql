CREATE TABLE [dbo].[Theses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](400) NOT NULL,
  [Description] [ntext] NOT NULL,
  [MaxStudentNumber] [int] NOT NULL,
  [Credits] [int] NOT NULL,
  [Year] [nvarchar](100) NOT NULL,
  [Notes] [nvarchar](200) NULL,
  [TopicId] [varchar](50) NOT NULL DEFAULT ('rkrKd_KtQJemI'),
  [TrainingFormId] [varchar](50) NULL,
  [TrainingLevelId] [varchar](50) NOT NULL DEFAULT ('sHUaEg-qBhVh'),
  [SpecializationId] [varchar](50) NOT NULL,
  [DateFrom] [datetime] NULL,
  [DateTo] [datetime] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [LectureId] [varchar](50) NOT NULL,
  [Semester] [int] NOT NULL,
  [ThesisGroupId] [varchar](50) NULL,
  [StatusId] [int] NOT NULL DEFAULT (1),
  [File] [nvarchar](500) NULL,
  CONSTRAINT [PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ID]),
  CHECK ([Semester]=(3) OR [Semester]=(2) OR [Semester]=(1))
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [ux_groupId_notnull]
  ON [dbo].[Theses] ([ThesisGroupId])
  WHERE ([ThesisGroupId] IS NOT NULL)
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_FacultyStaffs_ID] FOREIGN KEY ([LectureId]) REFERENCES [dbo].[FacultyStaffs] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Specializations_ID] FOREIGN KEY ([SpecializationId]) REFERENCES [dbo].[Specializations] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_ThesisGroups_ID] FOREIGN KEY ([ThesisGroupId]) REFERENCES [dbo].[ThesisGroups] ([ID]) ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_ThesisStatus_Id] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[ThesisStatus] ([Id])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Topics_ID] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topics] ([ID]) ON DELETE SET DEFAULT ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingForms_ID] FOREIGN KEY ([TrainingFormId]) REFERENCES [dbo].[TrainingForms] ([ID]) ON DELETE SET NULL ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingLevels_ID] FOREIGN KEY ([TrainingLevelId]) REFERENCES [dbo].[TrainingLevels] ([ID]) ON DELETE SET DEFAULT ON UPDATE CASCADE
GO