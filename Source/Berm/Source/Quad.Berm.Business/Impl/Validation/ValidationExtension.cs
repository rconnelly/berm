namespace Quad.Berm.Business.Impl.Validation
{
    using System.Linq;

    using FluentValidation;

    using Quad.Berm.Business.Exceptions;
    using Quad.Berm.Data;
    using Quad.Berm.Data.Common;
    using Quad.Berm.Persistence;

    internal static class ValidationExtension
    {
        public static void DemandValid<T>(this IValidator validator, T instance)
        {
            var result = validator.Validate(instance);
            if (!result.IsValid)
            {
                var failures = from error in result.Errors
                               where error != null
                               select
                                   new ValidationFailureInfo(
                                       error.PropertyName,
                                       error.ErrorMessage,
                                       error.CustomState as string,
                                       error.AttemptedValue,
                                       error.CustomState);
                throw new BusinessValidationException(failures.ToList());
            }
        }

        public static IRuleBuilderOptions<T, TProperty> Uniqness<T, TProperty, TBase>(
                this IRuleBuilderOptions<T, TProperty> ruleBuilder,
                IRepository repository,
                IInstanceQueryData<TBase> queryData)
            where T : class
            where TBase : class
        {
            var validator = new UniquePropertyValidator<TBase>(repository, queryData);
            ruleBuilder.SetValidator(validator);
            var result = ruleBuilder.WithErrorCode(MetadataInfo.InvalidState.AlreadyExist);
            return result;
        }

        public static IRuleBuilderOptions<T, TProperty> Existence<T, TProperty, TResult>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            IRepository repository)
                where T : class
                where TResult : class
        {
            var validator = new ExistingValuePropertyValidator<TResult, TProperty>(repository, new CommonGetQueryData<TResult, TProperty>());
            ruleBuilder.SetValidator(validator);
            var result = ruleBuilder.WithErrorCode(MetadataInfo.InvalidState.NotExist);
            return result;
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorCode<T, TProperty>(
                this IRuleBuilderOptions<T, TProperty> ruleBuilder,
                MetadataInfo.InvalidState invalidState)
            where T : class
        {
            var result = ruleBuilder.WithState(_ => invalidState.ToString("G"));
            return result;
        }
    }
}