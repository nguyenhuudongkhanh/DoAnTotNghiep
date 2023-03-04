CREATE TABLE [dbo].[FacultyStaffs] (
  [ID] [varchar](50) NOT NULL,
  [FacultyId] [varchar](50) NOT NULL DEFAULT ('BHyFWSywrk'),
  [Description] [ntext] NULL,
  [Gender] [nvarchar](10) NULL DEFAULT ('Nam'),
  [Phone] [varchar](20) NULL,
  [Address] [nvarchar](50) NULL,
  [Email] [nvarchar](50) NOT NULL,
  [Birthday] [date] NULL,
  [Avatar] [varchar](200) NULL,
  [Position] [nvarchar](50) NULL,
  [Degree] [nvarchar](50) NULL,
  [Notes] [nvarchar](200) NULL,
  [Password] [nvarchar](100) NOT NULL,
  [Salt] [nvarchar](100) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [VerificationCode] [varchar](100) NULL,
  [CodeExpTime] [datetime] NULL,
  [Surname] [nvarchar](200) NULL,
  [Name] [nvarchar](100) NULL,
  CONSTRAINT [PK_FacultyStaff_ID] PRIMARY KEY CLUSTERED ([ID]),
  UNIQUE ([Email]),
  CHECK ([Gender]=N'Nữ' OR [Gender]='Nam')
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [ux_fsphone_notnull]
  ON [dbo].[FacultyStaffs] ([Phone])
  WHERE ([Phone] IS NOT NULL)
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[FacultyStaffs]
  ADD CONSTRAINT [FK_FacultyStaffs_Faculties_ID] FOREIGN KEY ([FacultyId]) REFERENCES [dbo].[Faculties] ([ID]) ON DELETE SET DEFAULT ON UPDATE CASCADE
GO