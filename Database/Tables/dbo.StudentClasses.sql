CREATE TABLE [dbo].[StudentClasses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [nvarchar](200) NULL,
  [StudentQuantity] [int] NULL,
  [FacultyId] [varchar](50) NOT NULL DEFAULT ('BHyFWSywrk'),
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_StudentClass_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentClasses]
  ADD CONSTRAINT [FK_StudentClasses_Faculties_ID] FOREIGN KEY ([FacultyId]) REFERENCES [dbo].[Faculties] ([ID]) ON DELETE SET DEFAULT ON UPDATE CASCADE
GO