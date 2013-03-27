namespace Quad.Berm.Persistence.Impl.Commands
{
    using System.Collections.Generic;

    using Quad.Berm.Data.Common;

    internal class RelationalSpecificationCommand<TResult> : RelationalQueryCommand<ISpecification<TResult>, TResult>
    {
        public override IEnumerable<TResult> Execute(ISpecification<TResult> queryData)
        {
            var result = this.Session
                .Apply(queryData);
            return result;
        }

        public override int RowCount(ISpecification<TResult> queryData)
        {
            var result = this.Session
                .Count(queryData);
            return result;
        }
    }
}