DELETE FROM [berm].[User] WHERE [UserId] < 5
DELETE FROM [berm].[Role] WHERE [RoleId] < 5
DELETE FROM [berm].[Client] WHERE [ClientId] < 2
DELETE FROM [berm].[OrganizationGroup] WHERE [OrganizationGroupId] < 4
DELETE FROM [berm].[Permission] WHERE [PermissionId] < 4

set IDENTITY_INSERT [berm].[Permission] on
INSERT INTO [berm].[Permission]([PermissionId],[Name],[Created]) VALUES 
	(1, 'ManageSuperAdmin', getdate()),
    (2, 'ManageLocalAdmin', getdate()),
    (3, 'ManageUser', getdate())    
set IDENTITY_INSERT [berm].[Permission] off


set IDENTITY_INSERT [berm].[OrganizationGroup] on
INSERT INTO [berm].[OrganizationGroup] ([OrganizationGroupId],[Hierarchy],[Name],[Created])VALUES
     (1,'/','TestClient',getdate()),
     (2,'/1/','Group1',getdate()),
     (3,'/2/','Group2',getdate())
set IDENTITY_INSERT [berm].[OrganizationGroup] off


set IDENTITY_INSERT [berm].[Client] on
INSERT INTO [berm].[Client]([ClientId],[OrganizationGroupId],[Name],[Disabled],[Created])VALUES 
	(1, 1, 'TestClient', 0, getdate())
set IDENTITY_INSERT [berm].[Client] off


set IDENTITY_INSERT [berm].[Role] on
INSERT INTO [berm].[Role]([RoleId],[ClientId],[Name],[Created])VALUES
	(1,null,'SuperAdmin',getdate()),
    (2,1,'Admin',getdate()),
    (3,1,'Group1',getdate()),
    (4,1,'Group2',getdate())
set IDENTITY_INSERT [berm].[Role] off


INSERT INTO [berm].[OrganizationGroupRole]([OrganizationGroupId],[RoleId])VALUES
    (2,3),
    (3,4)


INSERT INTO [berm].[PermissionRole]([RoleId],[PermissionId])VALUES
	(1,1),
    (1,3),
    (2,2),
    (2,3)


set IDENTITY_INSERT [berm].[User] on
INSERT INTO [berm].[User]([UserId],[RoleId],[Name],[Created])VALUES
	(1,1,'super',getdate()),
    (2,2,'admin',getdate()),
    (3,3,'user1',getdate()),
    (4,4,'user2',getdate())
set IDENTITY_INSERT [berm].[User] off


set IDENTITY_INSERT [berm].[StsCredential] on
INSERT INTO [berm].[StsCredential]([StsCredentialId], [UserId],[Provider],[Identifier],[Created])VALUES
	(1, 1,'Google','dmitriy@quad.io',getdate()),
    (2, 2,'Google','dmitriy+admin@quad.io',getdate()),
    (3, 3,'Google','dmitriy+user1@quad.io',getdate()),
    (4, 4,'Google','dmitriy+user2@quad.io',getdate())
set IDENTITY_INSERT [berm].[StsCredential] off