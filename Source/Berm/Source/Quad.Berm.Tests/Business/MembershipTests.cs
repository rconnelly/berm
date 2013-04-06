namespace Quad.Berm.Tests.Business
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;
    using System.Threading;

    using NHibernate;
    using NHibernate.Linq;

    using NUnit.Framework;

    using Quad.Berm.Business;
    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Common.Transactions;
    using Quad.Berm.Common.Unity;
    using Quad.Berm.Data;
    using Quad.Berm.Tests.Common;

    [TestFixture]
    public class MembershipTests : TestBase
    {
        private const int DefaultClientId = 1;

        [Test]
        public void FindUser()
        {
            using (CreateSuperAdminContext())
            {
                var manager = this.Locator.GetInstance<IUserManager>();
                var user = manager.FindActive(MetadataInfo.DefaultIdentityProvider, "dmitriy@quad.io");
                Assert.That(user, Is.Not.Null);
            }
        }

        /// <summary>
        /// SuperAdmin can see all users
        /// </summary>
        [Test]
        public void ListAllUsers()
        {
            using (CreateSuperAdminContext())
            {
                var manager = this.Locator.GetInstance<IUserManager>();
                var count = manager.Count();
                var users = manager.Enumerate();

                var session = this.Locator.GetInstance<ISession>();
                var databaseCount = session.Query<UserEntity>().Count(m => !m.Deleted);
                Assert.That(count, Is.EqualTo(databaseCount));
                if (count <= 100)
                {
                    Assert.That(users.Count(), Is.EqualTo(databaseCount));
                }
            }
        }

        /// <summary>
        /// Admin can enumerate only users for the same customer
        /// </summary>
        [Test]
        public void ListOwnedUsers()
        {
            using (CreateAdminContext())
            {
                var manager = this.Locator.GetInstance<IUserManager>();
                var count = manager.Count();
                var users = manager.Enumerate();
                
                var session = this.Locator.GetInstance<ISession>();
                var databaseCount = session
                    .Query<UserEntity>().Count(m => !m.Deleted && m.Role.Client.Id == DefaultClientId);
                Assert.That(count, Is.EqualTo(databaseCount));
                if (count <= 100)
                {
                    Assert.That(users.Count(), Is.EqualTo(databaseCount));
                }
            }
        }

        /// <summary>
        /// Only Admin and SuperAdmin can enumerate users
        /// </summary>
        [Test]
        public void NoRightsToEnumerateUsers()
        {
            using (CreateUserContext())
            {
                var manager = this.Locator.GetInstance<IUserManager>();
                Assert.Throws<SecurityException>(() => manager.Count());
                Assert.Throws<SecurityException>(() => manager.Enumerate());
            }
        }

        [Test]
        public void CreateSuperAdmin()
        {
            Action action = () =>
                {
                    using (new Transaction())
                    {
                        var manager = this.Locator.GetInstance<IUserManager>();
                        var session = this.Locator.GetInstance<ISession>();
                        var role =
                            session.Query<RoleEntity>()
                                   .First(r => r.Permissions.Any(p => p.Name == AccessRight.ManageSuperAdmin));

                        var user = new UserEntity
                                       {
                                           Role = role, 
                                           Name = this.ShortStringGenerator.GetRandomValue()
                                       };
                        manager.Create(user);
                    }
                };

            using (CreateSuperAdminContext())
            {
                action();
            }

            using (CreateAdminContext())
            {
                Assert.Throws<SecurityException>(() => action(), "LocalAdmin cannot create SuperAdmin");
            }

            using (CreateUserContext())
            {
                Assert.Throws<SecurityException>(() => action(), "User cannot create SuperAdmin");
            }
        }

        [Test]
        public void CreateLocalAdmin()
        {
            Action action = () =>
            {
                using (new Transaction())
                {
                    var manager = this.Locator.GetInstance<IUserManager>();
                    var session = this.Locator.GetInstance<ISession>();
                    var role =
                        session.Query<RoleEntity>()
                               .First(r => r.Permissions.Any(p => p.Name == AccessRight.ManageLocalAdmin));

                    var user = new UserEntity
                    {
                        Role = role,
                        Name = this.ShortStringGenerator.GetRandomValue()
                    };
                    manager.Create(user);
                }
            };

            using (CreateSuperAdminContext())
            {
                action();
            }

            using (CreateAdminContext())
            {
                action();
            }

            using (CreateUserContext())
            {
                Assert.Throws<SecurityException>(() => action(), "User cannot create LocalAdmin");
            }
        }

        [Test]
        public void CreateUser()
        {
            Action action = () =>
            {
                using (new Transaction())
                {
                    var manager = this.Locator.GetInstance<IUserManager>();
                    var session = this.Locator.GetInstance<ISession>();
                    var role =
                        session.Query<RoleEntity>()
                               .First(r => r.Permissions.All(p => p.Name != AccessRight.ManageLocalAdmin) && r.Permissions.All(p => p.Name != AccessRight.ManageSuperAdmin));

                    var user = new UserEntity
                    {
                        Role = role,
                        Name = this.ShortStringGenerator.GetRandomValue()
                    };
                    manager.Create(user);
                }
            };

            using (CreateSuperAdminContext())
            {
                action();
            }

            using (CreateAdminContext())
            {
                action();
            }

            using (CreateUserContext())
            {
                Assert.Throws<SecurityException>(() => action(), "User cannot create other users");
            }
        }

        [Test]
        public void DeleteUser()
        {
            Action action = () =>
            {
                using (new Transaction())
                {
                    var manager = this.Locator.GetInstance<IUserManager>();
                    var session = this.Locator.GetInstance<ISession>();
                    
                    var role = session.Query<RoleEntity>().First(r => r.Client != null);
                    
                    var user = new UserEntity
                    {
                        Role = role,
                        Name = this.ShortStringGenerator.GetRandomValue()
                    };
                    manager.Create(user);

                    manager.Delete(user);

                    session.Flush();
                    session.Clear();

                    user = session.Load<UserEntity>(user.Id);
                    Assert.That(user.Deleted, Is.True);
                }
            };

            using (CreateSuperAdminContext())
            {
                action();
            }

            using (CreateAdminContext())
            {
                action();
            }

            using (CreateContext("dmitriy+admin2@quad.io", "2", AccessRight.ManageLocalAdmin))
            {
                Assert.Throws<SecurityException>(() => action(), "Local Admin cannot delete otherc client's users");
            }

            using (CreateUserContext())
            {
                Assert.Throws<SecurityException>(() => action(), "User cannot delete other users");
            }
        }

        [Test]
        public void CreateUserCredentials()
        {
            using (CreateSuperAdminContext())
            {
                using (new Transaction())
                {
                    var manager = this.Locator.GetInstance<IUserManager>();
                    var session = this.Locator.GetInstance<ISession>();
                    var role = session.Query<RoleEntity>().First();

                    var user = new UserEntity
                                   {
                                       Role = role, 
                                       Name = this.ShortStringGenerator.GetRandomValue()
                                   };
                    var credential = new StsCredentialEntity
                                         {
                                             Provider = MetadataInfo.DefaultIdentityProvider,
                                             Identifier = this.ShortStringGenerator.GetRandomValue()
                                         };
                    user.AddCredential(credential);
                    manager.Create(user);
                    session.Flush();
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ExecutionConstraintException))]
        public void CreateDuplicatedUserCredentials()
        {
            using (CreateSuperAdminContext())
            {
                using (new Transaction())
                {
                    var manager = this.Locator.GetInstance<IUserManager>();
                    var session = this.Locator.GetInstance<ISession>();
                    var role = session.Query<RoleEntity>().First();

                    var user = new UserEntity
                    {
                        Role = role,
                        Name = this.ShortStringGenerator.GetRandomValue()
                    };
                    var credential = new StsCredentialEntity
                    {
                        Provider = MetadataInfo.DefaultIdentityProvider,
                        Identifier = this.ShortStringGenerator.GetRandomValue()
                    };
                    var credential2 = new StsCredentialEntity
                    {
                        Provider = credential.Provider,
                        Identifier = credential.Identifier
                    };
                    user.AddCredential(credential);
                    user.AddCredential(credential2);
                    manager.Create(user);
                    session.Flush();
                }
            }
        }

        private static AmbientContext CreateSuperAdminContext()
        {
            return CreateContext("dmitriy@quad.io", null, AccessRight.ManageSuperAdmin);
        }

        private static AmbientContext CreateAdminContext()
        {
            return CreateContext("dmitriy+admin@quad.io", DefaultClientId.ToString(CultureInfo.InvariantCulture), AccessRight.ManageLocalAdmin);
        }

        private static AmbientContext CreateUserContext()
        {
            return CreateContext("dmitriy+user1@quad.io", DefaultClientId.ToString(CultureInfo.InvariantCulture));
        }

        private static AmbientContext CreateContext(string value, string clientId, params AccessRight[] rights)
        {
            var identity = new ClaimsIdentity("test");
            identity.AddClaim(new Claim(MetadataInfo.ClaimTypes.IdentityProvider, MetadataInfo.DefaultIdentityProvider));
            identity.AddClaim(new Claim(MetadataInfo.ClaimTypes.Identity, value));
            if (!string.IsNullOrEmpty(clientId))
            {
                identity.AddClaim(new Claim(MetadataInfo.ClaimTypes.ClientId, clientId));
            }

            foreach (var accessRight in rights)
            {
                identity.AddClaim(new Claim(MetadataInfo.ClaimTypes.ToClaim(accessRight), "1"));
            }

            var principal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = principal;
            var context = new AmbientContext();

            return context;
        }
    }
}
