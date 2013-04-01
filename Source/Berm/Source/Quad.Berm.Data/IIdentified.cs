namespace Quad.Berm.Data
{
    public interface IIdentified<T>
    {
        T Id { get; set; }
    }

    public interface IIdentified : IIdentified<long>
    {
    }
}