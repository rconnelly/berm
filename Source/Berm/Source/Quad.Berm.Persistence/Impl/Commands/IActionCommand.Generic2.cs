namespace Quad.Berm.Persistence.Impl.Commands
{
    using Quad.Berm.Data.Common;

    internal interface IActionCommand<in TActionData> : IActionCommand
        where TActionData : IActionData
    {
        void Execute(TActionData queryData);
    }
}