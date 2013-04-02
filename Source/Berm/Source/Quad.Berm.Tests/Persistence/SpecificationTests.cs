namespace Quad.Berm.Tests.Persistence
{
    using Ach.Fulfillment.Data;

    using FluentNHibernate.Testing;

    using Microsoft.SqlServer.Types;

    using NUnit.Framework;

    using Quad.Berm.Data;

    [TestFixture]
    public class SpecificationTests : PersistenseTestsBase
    {
        #region Public Methods and Operators

        [Test]
        public void PermissionSpecification()
        {
            var permission = new PersistenceSpecification<PermissionEntity>(this.Session)
                .CheckProperty(c => c.Name, AccessRight.SuperAdmin)
            .VerifyTheMappings();

            Assert.That(permission, Is.Not.Null);
        }

        [Test]
        public void ClientSpecification()
        {
            var client = new PersistenceSpecification<ClientEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void RoleSpecification()
        {
            var client = new PersistenceSpecification<ClientEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            var role = new PersistenceSpecification<RoleEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
                .CheckEntity(c => c.Client, client)
            .VerifyTheMappings();

            Assert.That(role, Is.Not.Null);
        }

        [Test]
        public void UserSpecification()
        {
            var role = new PersistenceSpecification<RoleEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            var user =
                new PersistenceSpecification<UserEntity>(this.Session)
                    .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
                    .CheckEntity(c => c.Role, role)
            .VerifyTheMappings();

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void StsCredentialSpecification()
        {
            var role = new PersistenceSpecification<RoleEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            var user = 
                new PersistenceSpecification<UserEntity>(this.Session)
                    .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
                    .CheckEntity(c => c.Role, role)
            .VerifyTheMappings();

            var credential = new PersistenceSpecification<StsCredentialEntity>(this.Session)
                .CheckProperty(c => c.Provider, "Google")
                .CheckProperty(c => c.Identifier, "sample@quad.io")
                .CheckEntity(c => c.User, user)
            .VerifyTheMappings();

            Assert.That(credential, Is.Not.Null);
        }

        [Test]
        public void OrganizationGroupSpecification()
        {
            var organizationGroup = new PersistenceSpecification<OrganizationGroupEntity>(this.Session)
                .CheckProperty(c => c.Hierarchy, SqlHierarchyId.GetRoot())
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            Assert.That(organizationGroup, Is.Not.Null);
        }

        [Test]
        public void PermissionRoleSpecification()
        {
            var role = new PersistenceSpecification<RoleEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            var permission = new PersistenceSpecification<PermissionEntity>(this.Session)
                .CheckProperty(c => c.Name, AccessRight.User)
            .VerifyTheMappings();

            role = this.Session.Load<RoleEntity>(role.Id);
            role.Permissions.Add(permission);

            this.Session.Save(role);
            this.Session.Flush();
            this.Session.Clear();
        
            role = this.Session.Load<RoleEntity>(role.Id);

            Assert.That(role.Permissions.Count, Is.EqualTo(1));
        }

        [Test]
        public void OrganizationGroupRoleSpecification()
        {
            var role = new PersistenceSpecification<RoleEntity>(this.Session)
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            var organizationGroup = new PersistenceSpecification<OrganizationGroupEntity>(this.Session)
                .CheckProperty(c => c.Hierarchy, SqlHierarchyId.GetRoot())
                .CheckProperty(c => c.Name, this.ShortStringGenerator.GetRandomValue())
            .VerifyTheMappings();

            role = this.Session.Load<RoleEntity>(role.Id);
            role.OrganizationGroup.Add(organizationGroup);

            this.Session.Save(role);
            this.Session.Flush();
            this.Session.Clear();

            role = this.Session.Load<RoleEntity>(role.Id);

            Assert.That(role.OrganizationGroup.Count, Is.EqualTo(1));

            this.Session.Clear();

            organizationGroup = this.Session.Load<OrganizationGroupEntity>(organizationGroup.Id);

            Assert.That(organizationGroup.Roles.Count, Is.EqualTo(1));
        }

        #endregion
    }
}