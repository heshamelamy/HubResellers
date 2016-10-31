using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HubManPractices.Models
{
    public class Permission 
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}