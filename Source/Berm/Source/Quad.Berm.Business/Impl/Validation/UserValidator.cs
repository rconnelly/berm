namespace Quad.Berm.Business.Impl.Validation
{
    using System.Linq;

    using FluentValidation;

    using Quad.Berm.Data;

    internal class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            this.RuleFor(m => m.Name).NotEmpty().Length(1, MetadataInfo.StringNormal);
            this.RuleFor(m => m.Role).NotNull();
            this.RuleFor(m => m.Deleted).Must((m, deleted) => !deleted);
            this.RuleFor(m => m.StsCredentials)
                .Must(m => m.FirstOrDefault() != null && m.First().Identifier != null)
                .WithMessage("'Identifier' should not be empty.")
                .OverridePropertyName("Identifier");
        }
    }
}