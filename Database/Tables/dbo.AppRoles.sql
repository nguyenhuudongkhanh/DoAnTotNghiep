CREATE TABLE [dbo].[AppRoles] (
  [ID ] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_FacultyRoles_ID ] PRIMARY KEY CLUSTERED ([ID ])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO