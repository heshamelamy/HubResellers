using HubManPractices.Models;
using HubManPractices.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public interface IClientService
    {
        IEnumerable<ClientViewModel> MapToViewModel(IEnumerable<Client> Clients);
        void AddOfficeSubscription(Guid ClientID, Guid SubID, int UsersPerSub);
        ClientViewModel MapToViewModel(Client client);
        Client GetById(Guid ClientID);
        void CreateClient(Client client);
        void EditClient(Client client);
        bool Exists(Client client);
        bool ExistsAndDeleted(Client client);
        void SaveClient();

        void DeleteClient(Client ToBeDeleted);

        Client GetDeletedClient(Client client);
        bool NeedsRenewal(Client client);

        ClientSubscriptions GetClientSubscription(Guid ClientID, Guid SubID);

        bool ClientNameAndMailExists(string ClientName, string ContactMail);

        bool ClientNameAndMailExistsAndDeleted(string ClientName, string ContactMail);
        Client GetClientByNameAndMail(string ClientName, string ContactMail);

        IEnumerable<Client> GetClients();

    }
}
