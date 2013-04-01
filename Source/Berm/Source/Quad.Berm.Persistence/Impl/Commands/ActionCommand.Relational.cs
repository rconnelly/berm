namespace Quad.Berm.Persistence.Impl.Commands
{
    using Microsoft.Practices.Unity;

    using NHibernate;

    using Quad.Berm.Data.Specifications;

    internal abstract class RelationalActionCommand<TActionData> : ActionCommandBase<TActionData>
        where TActionData : IActionData
    {
        #region Public Properties

        [Dependency]
        public ISession Session { get; set; }

        #endregion
    }
}