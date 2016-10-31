using HubMan.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HubMan.Filters
{
    public class PermissionAuthFilter : AuthorizeAttribute
    {
        public string Permission { get; set; }
        public PermissionAuthFilter()
        {

        }

        public PermissionAuthFilter(string Permission)
        {
            this.Permission = Permission;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            using (var db = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(db);
                var manager = new ApplicationUserManager(store);

                ApplicationUser CurrentUser = manager.FindByName(httpContext.User.Identity.Name);
                if (manager.GetRoles(CurrentUser.Id).ElementAt(0) == null)
                    return false;
                var roleName = manager.GetRoles(CurrentUser.Id).ElementAt(0);
                var role = db.Roles.First(s => s.Name == roleName);
                var perm = role.Permissions.FirstOrDefault(s => s.PermissionName == Permission);
                if (perm != null)
                    return true;
                return false;
            }
            //check your permissions
        }
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