namespace Quad.Berm.Web.Mvc
{
    using System.Web.Mvc;

    using Quad.Berm.Mvc;

    public static class FilterConfig
    {              
        #region Public Methods and Operators

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute { Order = 5 });
            filters.Add(new AuthorizeAttribute { Order = 10 });
        }

        #endregion
    }
}