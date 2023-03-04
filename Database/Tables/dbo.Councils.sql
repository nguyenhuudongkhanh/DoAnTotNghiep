CREATE TABLE [dbo].[Councils] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [Semester] [int] NULL,
  [Year] [nvarchar](20) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_Councils_ID] PRIMARY KEY CLUSTERED ([ID]),
  CHECK ([Semester]=(3) OR [Semester]=(2) OR [Semester]=(1))
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO