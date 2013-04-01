namespace Quad.Berm.Mvc
{
    using System;
    using System.Web;

    public class FakeServerHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += PreSendRequestHeaders;
        }

        public void Dispose()
        {
        }

        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;

            // want to hide real value
            context.Response.Headers.Set("Server", "Apache 2.0");
        }
    }
}