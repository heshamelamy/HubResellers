using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public interface IRoleService
    {
        void CreateRole(Role role);
        void DeleteRole(Role role);
        void RemovePermission(string RoleName,Guid pID);
        void SaveRole();
        Permission HasPermission(string UserId,string PName);
        IEnumerable <ApplicationUser> GetUserInRole(string Role);

        Role GetById(string id);

        IEnumerable<Role> GetRoles();

    }
}
