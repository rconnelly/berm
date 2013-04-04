if exists (select 1
            from  sysobjects
           where  id = object_id('berm.StsCredential')
            and   type = 'U')
   drop table berm.StsCredential
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.PermissionRole')
            and   type = 'U')
   drop table berm.PermissionRole
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.OrganizationGroupRole')
            and   type = 'U')
   drop table berm.OrganizationGroupRole
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm."User"')
            and   type = 'U')
   drop table berm."User"
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.Role')
            and   type = 'U')
   drop table berm.Role
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.Client')
            and   type = 'U')
   drop table berm.Client
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.OrganizationGroup')
            and   type = 'U')
   drop table berm.OrganizationGroup
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('berm.Permission')
            and   type = 'U')
   drop table berm.Permission
GO

if exists(select * from sys.schemas where name = N'berm')
   drop schema berm
GO

/*==============================================================*/
/* User: berm                                                   */
/*==============================================================*/
create schema berm authorization dbo
GO

/*==============================================================*/
/* Table: Client                                                */
/*==============================================================*/
create table berm.Client (
   ClientId             int                  identity,
   OrganizationGroupId  int                  null,
   Name                 nvarchar(255)        not null,
   Disabled             bit                  not null constraint DF_DISABLED_CLIENT default 0,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_CLIENT primary key (ClientId)
)
GO

/*==============================================================*/
/* Index: UX_CLIENT                                             */
/*==============================================================*/
create unique index UX_CLIENT on berm.Client (
   Name ASC
)
GO

/*==============================================================*/
/* Table: OrganizationGroup                                     */
/*==============================================================*/
create table berm.OrganizationGroup (
   OrganizationGroupId  int                  identity,
   Hierarchy            HierarchyID          not null,
   Name                 nvarchar(255)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_ORGANIZATIONGROUP primary key (OrganizationGroupId)
)
GO

/*==============================================================*/
/* Table: OrganizationGroupRole                                 */
/*==============================================================*/
create table berm.OrganizationGroupRole (
   OrganizationGroupId  int                  not null,
   RoleId               int                  not null,
   constraint PK_ORGANIZATIONGROUPROLE primary key (OrganizationGroupId, RoleId)
)
GO

/*==============================================================*/
/* Table: Permission                                            */
/*==============================================================*/
create table berm.Permission (
   PermissionId         int                  identity,
   Name                 nvarchar(255)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_PERMISSION primary key (PermissionId)
)
GO

/*==============================================================*/
/* Index: UX_PERMISSION                                         */
/*==============================================================*/
create unique index UX_PERMISSION on berm.Permission (
   Name ASC
)
GO

/*==============================================================*/
/* Table: PermissionRole                                        */
/*==============================================================*/
create table berm.PermissionRole (
   RoleId               int                  not null,
   PermissionId         int                  not null,
   constraint PK_PERMISSIONROLE primary key (RoleId, PermissionId)
)
GO

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table berm.Role (
   RoleId               int                  identity,
   ClientId             int                  null,
   Name                 nvarchar(255)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_ROLE primary key (RoleId)
)
GO

/*==============================================================*/
/* Table: StsCredential                                         */
/*==============================================================*/
create table berm.StsCredential (
   StsCredentialId      int                  identity,
   UserId               int                  not null,
   Provider             nvarchar(255)        not null,
   Identifier           nvarchar(255)        not null,
   IdentifierHash       AS (cast (HASHBYTES('MD5', [Provider] + '|' + [Identifier]) AS VARBINARY(16))) persisted not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_STSCREDENTIAL primary key nonclustered (StsCredentialId)
)
GO

/*==============================================================*/
/* Index: UX_STSCREDENTIAL_IDENTIFIER                           */
/*==============================================================*/
create unique index UX_STSCREDENTIAL_IDENTIFIER on berm.StsCredential (
   IdentifierHash ASC
)
GO

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table berm."User" (
   UserId               int                  identity,
   RoleId               int                  not null,
   Name                 nvarchar(255)        not null,
   Disabled             bit                  not null constraint DF_DISABLED_USER default 0,
   Deleted              bit                  not null constraint DF_DELETED_USER default 0,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_USER primary key (UserId)
)
GO

alter table berm.Client
   add constraint FK_CLIENT_ORGANIZATIONGROUP foreign key (OrganizationGroupId)
      references berm.OrganizationGroup (OrganizationGroupId)
GO

alter table berm.OrganizationGroupRole
   add constraint FK_ORGANIZATIONGROUPROL_ORGANIZATIONGROUP foreign key (OrganizationGroupId)
      references berm.OrganizationGroup (OrganizationGroupId)
         on delete cascade
GO

alter table berm.OrganizationGroupRole
   add constraint FK_ORGANIZATIONGROUPROL_ROLE foreign key (RoleId)
      references berm.Role (RoleId)
         on delete cascade
GO

alter table berm.PermissionRole
   add constraint FK_PERMISSIONROLE_ROLE foreign key (RoleId)
      references berm.Role (RoleId)
         on delete cascade
GO

alter table berm.PermissionRole
   add constraint FK_PERMISSIONROLE_PERMISSION foreign key (PermissionId)
      references berm.Permission (PermissionId)
         on delete cascade
GO

alter table berm.Role
   add constraint FK_ROLE_CLIENT foreign key (ClientId)
      references berm.Client (ClientId)
         on delete cascade
GO

alter table berm.StsCredential
   add constraint FK_STSCREDENTIAL_USER foreign key (UserId)
      references berm."User" (UserId)
         on delete cascade
GO

alter table berm."User"
   add constraint FK_USER_ROLE foreign key (RoleId)
      references berm.Role (RoleId)
GO

