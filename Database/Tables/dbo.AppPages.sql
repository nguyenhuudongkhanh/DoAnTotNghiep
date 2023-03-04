CREATE TABLE [dbo].[AppPages] (
  [ID] [varchar](50) NOT NULL,
  [ControllerName] [varchar](100) NOT NULL,
  [ActionName] [varchar](100) NOT NULL,
  [Area] [varchar](100) NULL,
  [Path] [varchar](400) NULL,
  [PageName] [nvarchar](200) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_AppRoleMapping_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO