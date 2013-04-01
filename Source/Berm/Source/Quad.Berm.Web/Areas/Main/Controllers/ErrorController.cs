﻿namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult Error404()
        {
            this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            
            return this.View();
        }

        public ActionResult Error403()
        {
            this.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            return this.View();
        }

        [AllowAnonymous]
        public ActionResult Error401()
        {
            this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return this.View();
        }
    }
}
