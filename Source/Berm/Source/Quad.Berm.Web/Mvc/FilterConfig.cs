namespace Quad.Berm.Web.Mvc
{
    using System.Web.Mvc;

    using Quad.Berm.Mvc;

    public static class FilterConfig
    {              
        #region Public Methods and Operators

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }

        #endregion
    }
}