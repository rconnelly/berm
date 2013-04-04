namespace Quad.Berm.Data.Specifications.User
{
    public static class Users
    {
        public static IQueryData<UserEntity> All(int pageIndex = 0, int pageSize = 0)
        {
            return new UndeletedUser { PageIndex = pageIndex, PageSize = pageSize };
        }

        public static IQueryData<UserEntity> ByClientId(long clientId, int pageIndex = 0, int pageSize = 0)
        {
            return new UndeletedUserByClientId(clientId) { PageIndex = pageIndex, PageSize = pageSize };
        }

        public static IQueryData<UserEntity> ByCredentials(string provider, string identity)
        {
            return new ActiveUserByCredentials(provider, identity);
        }
    }
}