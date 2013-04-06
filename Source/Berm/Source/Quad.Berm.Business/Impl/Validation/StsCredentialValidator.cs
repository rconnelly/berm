namespace Quad.Berm.Business.Impl.Validation
{
    using FluentValidation;

    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications.User;
    using Quad.Berm.Persistence;

    internal class StsCredentialValidator : AbstractValidator<StsCredentialEntity>
    {
        public StsCredentialValidator(IRepository repository)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            this.RuleFor(m => m.User)
                .NotNull();
            this.RuleFor(m => m.Provider)
                .NotEmpty()
                .Length(1, MetadataInfo.StringNormal);
            this.RuleFor(m => m.Identifier)
                .NotEmpty()
                .Length(1, MetadataInfo.StringNormal)
                .Uniqness(repository, new UniqueStsCredentialIdentifier())
                .WithMessage("'{PropertyName}' already used.");
        }
    }
}