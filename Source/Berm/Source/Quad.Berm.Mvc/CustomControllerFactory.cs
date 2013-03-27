namespace Quad.Berm.Mvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using global::Common.Logging;

    using Microsoft.Practices.ServiceLocation;

    public class CustomControllerFactory<TErrorController> : DefaultControllerFactory
    {
        #region Constants and Fields

        private readonly ILog logger;

        #endregion

        #region Constructors

        public CustomControllerFactory()
            : base(new CustomControllerActivator())
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region Methods

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            IController controller;
            try
            {
                controller = base.GetControllerInstance(requestContext, controllerType);
            }
            catch (HttpException exc)
            {
                var httpCode = exc.GetHttpCode();
                if (httpCode == 404)
                {
                    this.logger.Error("Cannot instantiate mvc controller.", exc);

                    controller = base.GetControllerInstance(requestContext, typeof(TErrorController));

                    requestContext.RouteData.Values.Clear();

                    RouteHelper.InitErrorRoute(httpCode, requestContext.RouteData);
                }
                else
                {
                    throw;
                }
            }

            return controller;
        }

        #endregion

        private class CustomControllerActivator : IControllerActivator
        {
            #region Implementation of IControllerActivator

            public IController Create(RequestContext requestContext, Type controllerType)
            {
                var service = (IController)ServiceLocator.Current.GetInstance(controllerType);
                return service;
            }

            #endregion
        }
    }
}