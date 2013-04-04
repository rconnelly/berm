namespace Quad.Berm.Mvc
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;

    internal class ServiceLocatorControllerActivator : IControllerActivator
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