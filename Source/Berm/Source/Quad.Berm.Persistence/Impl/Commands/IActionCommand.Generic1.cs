namespace Quad.Berm.Persistence.Impl.Commands
{
    using Quad.Berm.Data.Specifications;

    internal interface IActionCommand
    {
        void Execute(IActionData queryData);
    }
}