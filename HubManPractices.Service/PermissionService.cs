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
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository permissionsRepository;
        private readonly IRoleRepository rolesRepositroy;
        private readonly ApplicationEntities ApplicationEntity;

        public PermissionService(ApplicationEntities Ae, IPermissionRepository PR,IRoleRepository RR)
        {
            this.permissionsRepository = PR;
            this.ApplicationEntity = Ae;
            this.rolesRepositroy = RR;
        }
        public void AssignPermissionToRole(string RoleName, Guid PID)
        {
            Role role = rolesRepositroy.GetRoleByName(RoleName);
            permissionsRepository.GetById(PID).Roles.Add(role);
        }

        public void CreatePermission(Permission permission)
        {
            permissionsRepository.Add(permission);
        }

        public void DeletePermission(Permission permission)
        {
            permissionsRepository.Delete(permission);
        }

        public void SavePermission()
        {
            ApplicationEntity.Commit();
        }

        public Permission GetById(Guid id)
        {
            return permissionsRepository.GetById(id);
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return permissionsRepository.GetAll();
        }
    }
}
