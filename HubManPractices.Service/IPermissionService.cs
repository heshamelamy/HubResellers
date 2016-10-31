using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public interface IPermissionService
    {
        void CreatePermission(Permission permission);
        void DeletePermission(Permission permission);
        void SavePermission();
        void AssignPermissionToRole(string RID, Guid PID);
    }
}
