using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubManPractices.Models;
using HubManPractices.Repository.Repositories;
using HubManPractices.Repository;

namespace HubManPractices.Service
{
    public class RoleService : IRoleService
    {

        private readonly IRoleRepository rolesRepository;

        private readonly ApplicationEntities ApplicationEntity;

        public RoleService(ApplicationEntities Ae, IRoleRepository RR)
        {
            this.rolesRepository = RR;
            this.ApplicationEntity = Ae;
        }

        public void CreateRole(Role role)
        {
            rolesRepository.Add(role);
        }

        public void DeleteRole(Role role)
        {
            rolesRepository.Delete(role);
        }

        public Permission HasPermission(string UserId,string PName)
        {
            ApplicationUserRole ur = rolesRepository.GetUserRoles(UserId);
            Role MyRole = rolesRepository.GetById(ur.RoleId);

            IEnumerable<Permission> MyPermissions = MyRole.Permissions.AsEnumerable();
        
            return MyPermissions.Where(p => p.PermissionName == PName).FirstOrDefault();
        }

        public void RemovePermission(string RoleName,Guid pID)
        {
            rolesRepository.RemovePermission(RoleName, pID);
        }

        public void SaveRole()
        {
            ApplicationEntity.Commit();
        }


        public IEnumerable<ApplicationUser> GetUserInRole(string Role)
        {
            return rolesRepository.GetUserInRole(Role);
        }

        public Role GetById(string id)
        {
            return rolesRepository.GetById(id);
        }

        public IEnumerable<Role> GetRoles()
        {
            return rolesRepository.GetAll();
        }
    }
}
