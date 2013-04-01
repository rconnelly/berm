namespace Quad.Berm.Persistence.Impl.Commands
{
    using Quad.Berm.Data.Specifications;

    internal interface IActionCommand<in TActionData> : IActionCommand
        where TActionData : IActionData
    {
        void Execute(TActionData queryData);
    }
}