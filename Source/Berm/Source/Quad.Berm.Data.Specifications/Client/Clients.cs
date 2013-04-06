namespace Quad.Berm.Data.Specifications.Client
{
    public static class Clients
    {
        public static IQueryData<ClientEntity> All()
        {
            return new ClientById();
        }
    }
}