using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
         void AddOfficeSubscription(Guid ClientID, Guid SubID, int UsersPerSub);
          bool CheckIfExists(Client client);
          bool CheckIfExistsAndDeleted(Client client);

          Client GetDeletedClient(Client client);
          bool NeedsRenewal(Client client);

          ClientSubscriptions GetClientSubscription(Guid ClientID, Guid SubID);

          bool ClientNameAndMailExists(string ClientName, string ContactMail);
          bool ClientNameAndMailExistsAndDeleted(string ClientName, string ContactMail);

          Client GetByNameAndMail(string ClientName, string ContactMail);

    }
}
