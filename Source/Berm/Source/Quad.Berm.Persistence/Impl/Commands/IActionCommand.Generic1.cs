namespace Quad.Berm.Persistence.Impl.Commands
{
    using Quad.Berm.Data.Common;

    internal interface IActionCommand
    {
        void Execute(IActionData queryData);
    }
}