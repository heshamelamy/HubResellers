using System.Web.Mvc;

namespace Web.Filters
{
    public class MyAuthFilter : AuthorizeAttribute
    {
        

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                if (filterContext.HttpContext.Request.IsAuthenticated)
                    filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
                else
                    filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}