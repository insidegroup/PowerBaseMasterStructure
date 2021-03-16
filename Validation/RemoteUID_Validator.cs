using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CWTDesktopDatabase.Validation
{
    // <snippet1>
    public class RemoteValidator : DataAnnotationsModelValidator<RemoteUID_Attribute>
    {

        public RemoteValidator(ModelMetadata metadata, ControllerContext context,
            RemoteUID_Attribute attribute) :
            base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "remoteVal"
            };

            rule.ValidationParameters["url"] = GetUrl();
            rule.ValidationParameters["parameterName"] = Attribute.ParameterName;
            return new ModelClientValidationRule[] { rule };
        }

        private string GetUrl()
        {
            RouteValueDictionary rvd = new RouteValueDictionary() {
            { "controller", Attribute.Controller },
            { "action", Attribute.Action }
        };

            var virtualPath = RouteTable.Routes.GetVirtualPath(ControllerContext.RequestContext,
                Attribute.RouteName, rvd);
            if (virtualPath == null)
            {
                throw new InvalidOperationException("No route matched!");
            }

            return virtualPath.VirtualPath;
        }
    }
    // </snippet1>

}
