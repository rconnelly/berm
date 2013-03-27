namespace Quad.Berm.Persistence.Impl.Commands
{
    using Quad.Berm.Data.Common;

    internal abstract class ActionCommandBase<TActionData> : IActionCommand<TActionData>
        where TActionData : IActionData
    {
        public abstract void Execute(TActionData actionData);

        void IActionCommand.Execute(IActionData actionData)
        {
            this.Execute((TActionData)actionData);
        }
    }
}