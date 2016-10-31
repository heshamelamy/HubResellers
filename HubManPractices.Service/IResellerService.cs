using HubManPractices.Models;
using HubManPractices.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public interface IResellerService
    {
        IEnumerable<Reseller> GetResellers();
        Reseller GetById(Guid ResellerID);
        void EditReseller(Reseller reseller);
        IEnumerable<ResellerViewModel> MapToViewModel(IEnumerable<Reseller> resellers);
        ResellerViewModel MapToViewModel(Reseller reseller);
        IEnumerable<Client> GetResellerClients(Guid ResellerID);
        IEnumerable<Reseller> GetUserReseller(string UserID);
        void CreateReseller(Reseller r);
        void DeleteReseller(Reseller ToBeDeleted);

        bool QuotaFull(Guid ResellerID);
        IEnumerable<Reseller> SearchForResellers(string Query);
        IEnumerable<Client> GetResellerDeletedClients(Guid resellerID);
        string GetResellerImage(Guid ResellerID);
        IEnumerable<Client> GetRecent10Clients(IEnumerable<Client> clients);
        Dictionary<string, int> GetChartData(Guid ResellerID);
        void AddResellerUser(Reseller r, string UserMail);
    }
}
