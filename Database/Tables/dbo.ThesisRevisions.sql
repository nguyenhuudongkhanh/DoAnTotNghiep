CREATE TABLE [dbo].[ThesisRevisions] (
  [ID] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [Title] [nvarchar](100) NOT NULL,
  [Summary] [nvarchar](450) NULL,
  [DocumentFile] [nvarchar](max) NULL,
  [PresentationFile] [nvarchar](max) NULL,
  [PdfFile] [nvarchar](max) NULL,
  [SourceCode] [nvarchar](max) NULL,
  [Reviewed] [bit] NOT NULL,
  [LecturerComment] [nvarchar](450) NULL,
  [Point] [decimal](5, 2) NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_ThesisRevisions_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ThesisRevisions]
  ADD CONSTRAINT [FK_ThesisRevisions_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID]) ON DELETE CASCADE
GO