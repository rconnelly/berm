namespace Quad.Berm.Business.Impl.Validation
{
    using FluentValidation;

    using Quad.Berm.Data;

    internal class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator()
        {
            this.RuleFor(m => m.Name).NotEmpty().Length(1, MetadataInfo.StringNormal);
            this.RuleFor(m => m.Role).NotNull();
            this.RuleFor(m => m.Deleted).Must((m, deleted) => !deleted);
        }
    }
}