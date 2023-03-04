CREATE TABLE [dbo].[AppRoleMapping] (
  [RoleId] [varchar](50) NOT NULL,
  [PageId] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  CONSTRAINT [PK_AppRoleMapping] PRIMARY KEY CLUSTERED ([RoleId], [PageId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[AppRoleMapping]
  ADD CONSTRAINT [FK_AppRoleMapping_AppPages_ID] FOREIGN KEY ([PageId]) REFERENCES [dbo].[AppPages] ([ID])
GO

ALTER TABLE [dbo].[AppRoleMapping]
  ADD CONSTRAINT [FK_AppRoleMapping_AppRoles_ID ] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AppRoles] ([ID ])
GO