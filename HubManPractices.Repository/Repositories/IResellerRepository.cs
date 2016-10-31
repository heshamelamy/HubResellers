using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public interface IResellerRepository : IRepository<Reseller>
    {
        IEnumerable<Reseller> GetUserReseller(string UserId);

        IEnumerable<Reseller> SearchForResellers(string Query);

        IEnumerable<Client> GetResellerClients(Guid ResellerID);
        bool QuotaFull(Guid ResellerID);
        IEnumerable<Client> GetResellerDeletedClients(Guid resellerID);
        string GetResellerImage(Guid ResellerID);
        void AddResellerUser(Reseller reseller, string UserMail);
    }
}
