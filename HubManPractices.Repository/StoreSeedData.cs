using HubManPractices.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace HubManPractices.Repository
{
    public class StoreSeedData : DropCreateDatabaseIfModelChanges<ApplicationEntities>
    {
        private static Role GlobalRole = new Role { Name = "Global Admin", Permissions = new List<Permission>() };
        private static Role ResellerRole = new Role {Name = "Reseller Admin", Permissions = new List<Permission>() };


        protected override void Seed(ApplicationEntities context)
        {

            AddRolesPermissions(context);
            AddUserAndRoles(context);
            AddSubscriptions(context);
            context.Commit();

            //base.Seed(context);
        }

        private static void AddSubscriptions(ApplicationEntities context)
        {
            var ProPlus = new OfficeSubscription() { SubscriptionID = Guid.NewGuid(), SubscriptionName = "Office Pro Plus", MonthlyFee = 12 };
            var E1 = new OfficeSubscription() { SubscriptionID = Guid.NewGuid(), SubscriptionName = "Office E1", MonthlyFee = 8 };
            var E3 = new OfficeSubscription() { SubscriptionID = Guid.NewGuid(), SubscriptionName = "Office E3", MonthlyFee = 20 };
            var E5 = new OfficeSubscription() { SubscriptionID = Guid.NewGuid(), SubscriptionName = "Office E5", MonthlyFee = 33 };
            //var Unknown = new OfficeSubscription() { SubscriptionID = Guid.NewGuid(), SubscriptionName = "UnKnown Licenses", MonthlyFee = 0 };

            context.OfficeSubscriptions.AddOrUpdate(ProPlus);
            context.OfficeSubscriptions.AddOrUpdate(E1);
            context.OfficeSubscriptions.AddOrUpdate(E3);
            context.OfficeSubscriptions.AddOrUpdate(E5);
            //context.OfficeSubscriptions.AddOrUpdate(Unknown);
        }


        private static void AddUserAndRoles(ApplicationEntities context)
        {
            var GlobalAdmin = new ApplicationUser() { Email = "heshamelelamy@gmail.com", SecurityStamp = Guid.NewGuid().ToString(), UserName = "AmrElSehemy", PasswordHash = new PasswordHasher().HashPassword("global123") };
            var GlobalAdmin2 = new ApplicationUser() { Email = "heshamelelamy@gmail.com", SecurityStamp = Guid.NewGuid().ToString(), UserName = "OmarElSakka", PasswordHash = new PasswordHasher().HashPassword("global123") };
           


            //password must be greater than 6 chars
            context.Users.AddOrUpdate(GlobalAdmin);
            context.Users.AddOrUpdate(GlobalAdmin2);

            context.ApplicationUserRoles.AddOrUpdate(t => t.UserId, new ApplicationUserRole() { RoleId = GlobalRole.Id, UserId = GlobalAdmin.Id });
            context.ApplicationUserRoles.AddOrUpdate(t => t.UserId, new ApplicationUserRole() { RoleId = GlobalRole.Id, UserId = GlobalAdmin2.Id });
        }

        private static void AddRolesPermissions(ApplicationEntities context)
        {
            var AddReseller = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Add Reseller" };
            var AddClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Add Client" };
            var EditClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Edit Client" };
            var EditReseller = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Edit Reseller" };
            var DeleteReseller = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Delete Reseller" };
            var DeleteClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Delete Client" };
            var SuspendClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Suspend Client" };
            var ActivateClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Activate Client" };
            var UpgradeClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Upgrade Client" };
            var RenewClient = new Permission { PermissionId = Guid.NewGuid(), PermissionName = "Renew Client" };
            

            List<Permission> permissions = new List<Permission>() {
                AddReseller,
                AddClient,
                EditReseller,
                EditClient,
                DeleteReseller,
                DeleteClient,
                SuspendClient,
                ActivateClient,
                UpgradeClient,
                RenewClient,
            };


            try
            {

                //Add permissions to db and give them all to the global admin
                foreach (var perm in permissions)
                {
                    GlobalRole.Permissions.Add(perm);
                }


                ResellerRole.Permissions.Add(AddClient);
                ResellerRole.Permissions.Add(EditClient);
                ResellerRole.Permissions.Add(SuspendClient);
                ResellerRole.Permissions.Add(DeleteClient);
                ResellerRole.Permissions.Add(EditReseller);
                ResellerRole.Permissions.Add(ActivateClient);
                ResellerRole.Permissions.Add(UpgradeClient);
                ResellerRole.Permissions.Add(RenewClient);


                context.Roles.AddOrUpdate(t => t.Name, GlobalRole);
                context.Roles.AddOrUpdate(t => t.Name, ResellerRole);

            }
            catch (Exception ex)
            {
                
            }
            
        }
    }
}
