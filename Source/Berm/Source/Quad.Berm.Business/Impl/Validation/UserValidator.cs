namespace Quad.Berm.Business.Impl.Validation
{
    using FluentValidation;

    using Quad.Berm.Data;
    using Quad.Berm.Persistence;

    internal class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator(IRepository repository)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            this.RuleFor(m => m.Name).NotEmpty().Length(1, MetadataInfo.StringNormal);
            this.RuleFor(m => m.Role).NotNull();
            this.RuleFor(m => m.Deleted).Must((m, deleted) => !deleted);
            this.RuleFor(m => m.StsCredentials)
                .NotEmpty()
                .SetCollectionValidator(new StsCredentialValidator(repository))
                .OverridePropertyName("Identifier");
        }
    }
}