using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Configuration
{
    class ApplicationUserRoleConfiguration : EntityTypeConfiguration<ApplicationUserRole>
    {
        public ApplicationUserRoleConfiguration()
        {
            ToTable("AspNetUserRoles");
            HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId });
        }
    }
}
