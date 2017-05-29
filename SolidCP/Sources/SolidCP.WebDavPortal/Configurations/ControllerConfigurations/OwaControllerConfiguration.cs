using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using SolidCP.WebDavPortal.Configurations.ActionSelectors;

namespace SolidCP.WebDavPortal.Configurations.ControllerConfigurations
{
    public class OwaControllerConfiguration : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Services.Replace(typeof(IHttpActionSelector), new OwaActionSelector());
        }
    }
}