CREATE TABLE [dbo].[AppUserRoles] (
  [UserId] [varchar](50) NOT NULL,
  [RoleId] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL
)
ON [PRIMARY]

GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [PK_AppUserRoles_UserId] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_AppRoles_ID ] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AppRoles] ([ID ]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_FacultyStaffs_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[FacultyStaffs] ([ID]) ON DELETE CASCADE