namespace Quad.Berm.Business
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using Quad.Berm.Data;

    public interface IUserManager
    {
        int Count();

        IEnumerable<UserEntity> Enumerate(int pageIndex = 0, int pageSize = 0);

        UserEntity FindActive(string provider, string identifier);

        UserEntity Create(UserEntity user);

        void Update(UserEntity user);

        void Delete(UserEntity user);
    }
}