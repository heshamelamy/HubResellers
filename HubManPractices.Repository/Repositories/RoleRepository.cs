using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory) : base(dbFactory){}

        public void RemovePermission(string RoleName, Guid pID)
        {
            Permission PtoRemove = DbContext.Permissions.Where(p => p.PermissionId == pID).FirstOrDefault();
            GetRoleByName(RoleName).Permissions.Remove(PtoRemove);
            DbContext.Commit();
        }

        public Role GetRoleByName(string RoleName)
        {
            return DbContext.Roles.Where(r => r.Name == RoleName).FirstOrDefault();
        }

        public ApplicationUserRole GetUserRoles(string UserId)
        {
            return DbContext.ApplicationUserRoles.Where(u => u.UserId == UserId).FirstOrDefault();
        }


        public IEnumerable<ApplicationUser> GetUserInRole(string Role)
        {
            Role role = DbContext.Roles.Where(r=>r.Name==Role).FirstOrDefault();
            IEnumerable <ApplicationUserRole> UR = DbContext.ApplicationUserRoles.Where(r => r.RoleId == role.Id);
             List<ApplicationUser> GlobalAdmins= new List<ApplicationUser>();
            foreach(var user in UR)
            {
               GlobalAdmins.Add(DbContext.Users.Where(U => U.Id == user.UserId).FirstOrDefault());
            }
            return GlobalAdmins.AsEnumerable();
        }
    }

}
