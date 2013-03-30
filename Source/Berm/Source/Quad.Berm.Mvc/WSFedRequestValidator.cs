namespace Quad.Berm.Mvc
{
    using System;
    using System.IdentityModel.Services;
    using System.Web;
    using System.Web.Util;

    public class WSFedRequestValidator : RequestValidator
    {
        protected override bool IsValidRequestString(
            HttpContext context,
            string value,
            RequestValidationSource requestValidationSource,
            string collectionKey,
            out int validationFailureIndex)
        {
            bool isValid;
            
            if (requestValidationSource == RequestValidationSource.Form &&
                collectionKey.Equals(
                    "wresult"/*WSFederationConstants.Parameters.Result*/,
                    StringComparison.Ordinal))
            {
                validationFailureIndex = 0;
                var request = new HttpRequestWrapper(context.Request);
                var message = WSFederationMessage.CreateFromFormPost(request);
                var signInResponseMessage = message as SignInResponseMessage;
                isValid = signInResponseMessage != null;
            }
            else
            {
                isValid = base.IsValidRequestString(
                    context,
                    value,
                    requestValidationSource,
                    collectionKey,
                    out validationFailureIndex);
            }

            return isValid;
        }
    }
}