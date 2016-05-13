using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Coder.Helpers
{
    /*
    * Code snippet is taken directly from Mark on Stackoverflow
    * http://stackoverflow.com/questions/13284729/asp-net-mvc-4-custom-authorize-attribute-how-to-redirect-unauthorized-users-to
    */
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Forbidden" }));
            }
        }
    }
}