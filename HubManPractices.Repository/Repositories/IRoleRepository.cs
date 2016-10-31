using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        void RemovePermission(string RoleName, Guid pID);
        Role GetRoleByName(string RoleName);
        ApplicationUserRole GetUserRoles(string UserId);

        IEnumerable <ApplicationUser> GetUserInRole(string Role);
    }
}
