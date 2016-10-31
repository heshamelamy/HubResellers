using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public class ResellerRepository : RepositoryBase<Reseller>, IResellerRepository
    {
        public ResellerRepository(IDbFactory dbFactory) : base(dbFactory){}


        public override void Add(Reseller reseller)
        {
            Reseller Found = DbContext.Resellers.Where(u => u.Name == reseller.Name).Where(u => u.IsDeleted == true).FirstOrDefault();
            if (Found != null)
            {
                Found.IsDeleted = false;
                Found.ClientsQuota = reseller.ClientsQuota;
                DbContext.Commit();
            }
            else
            {
                string src = "~/Content/Images/" + reseller.Name+".PNG";
                reseller.ResellerImage = src;
                DbContext.Resellers.Add(reseller);
                DbContext.Commit();
            }
        }

        public void AddResellerUser(Reseller reseller,string UserMail)
        {
            var ResellerAdmin = new ApplicationUser() { Email=UserMail ,SecurityStamp = Guid.NewGuid().ToString(), UserName = reseller.Name + "admin", PasswordHash = new PasswordHasher().HashPassword("reseller123"), ResellerID = reseller.ResellerID, Reseller = reseller };
            DbContext.Users.Add(ResellerAdmin);
            Role ResellerRole = DbContext.Roles.Where(r => r.Name == "Reseller Admin").FirstOrDefault();
            DbContext.ApplicationUserRoles.Add(new ApplicationUserRole() { RoleId = ResellerRole.Id, UserId = ResellerAdmin.Id });
            DbContext.Commit();
        }

        public override void Delete(Reseller reseller)
        {
            Reseller ToBeDeleted = Get(r=>r.ResellerID==reseller.ResellerID);
            ToBeDeleted.IsDeleted = true;
            DbContext.Commit();
        }
        public IEnumerable<Reseller> GetUserReseller(string UserId)
        {
            if (DbContext.Users.Find(UserId).Reseller.IsDeleted.Equals(false))
            {
                return new List<Reseller>() { DbContext.Users.Find(UserId).Reseller }.AsEnumerable();
            }
            else return new List<Reseller>();
        }

        public IEnumerable<Reseller> SearchForResellers(string Query)
        {
            return GetMany(u => u.Name.StartsWith(Query) && u.IsDeleted == false);
        }

        public IEnumerable<Client> GetResellerClients(Guid ResellerID)
        {
            return this.DbContext.Resellers.Find(ResellerID).Clients.Where(c => c.IsDeleted == false);
        }
        public bool QuotaFull(Guid ResellerID)
        {
            Reseller reseller = GetById(ResellerID);
            if (reseller.ClientsQuota == GetResellerClients(ResellerID).Count())
                return true;
            else return false;

        }

        public IEnumerable<Client> GetResellerDeletedClients(Guid resellerID)
        {
            return this.DbContext.Resellers.Find(resellerID).Clients.Where(c => c.IsDeleted == true);
        }

        public string GetResellerImage(Guid ResellerID)
        {
            return DbContext.Resellers.Where(r => r.ResellerID == ResellerID).FirstOrDefault().ResellerImage;
        }
    }
}
