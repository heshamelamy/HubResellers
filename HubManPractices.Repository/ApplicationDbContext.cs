using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using HubManPractices.Models;
using HubManPractices.Repository.Configuration;

namespace HubManPractices.Repository
{
    public class ApplicationEntities : IdentityDbContext<ApplicationUser>
    {
        public ApplicationEntities() : base("BestPractices") { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Reseller> Resellers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ClientSubscriptions> ClientSubscriptions { get; set; }
        public DbSet<OfficeSubscription> OfficeSubscriptions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public static ApplicationEntities Create()
        {
            return new ApplicationEntities();
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }


            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserRoleConfiguration());
            modelBuilder.Configurations.Add(new ClientSubscriptionsConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());

        }


    }
}
