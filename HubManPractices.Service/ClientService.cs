using AutoMapper;
using HubManPractices.Models;
using HubManPractices.Repository;
using HubManPractices.Repository.Repositories;
using HubManPractices.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public class ClientService : IClientService
    {
        
        private readonly IClientRepository clientsRepository;

        private readonly ApplicationEntities ApplicationEntity;
        public ClientService(ApplicationEntities Ae, IClientRepository CR)
        {
            this.clientsRepository = CR;
            this.ApplicationEntity = Ae;
        }
        public IEnumerable<ClientViewModel> MapToViewModel(IEnumerable<Client> Clients)
        {
            return Mapper.Map<IEnumerable<Client>, IEnumerable<ClientViewModel>>(Clients);
        }

        public ViewModels.ClientViewModel MapToViewModel(Models.Client client)
        {
            return Mapper.Map<Client, ClientViewModel>(client);
        }

        public Client GetById(Guid ClientID)
        {
            return clientsRepository.GetById(ClientID);
        }

        public void CreateClient(Client c)
        {
            clientsRepository.Add(c);
        }
        public void EditClient(Client client)
        {
            clientsRepository.Update(client);
        }
        public void SaveClient()
        {
            ApplicationEntity.Commit();
        }


        public void DeleteClient(Client ToBeDeleted)
        {
            clientsRepository.Delete(ToBeDeleted);
        }


        public void AddOfficeSubscription(Guid ClientID, Guid SubID, int UsersPerSub)
        {
            clientsRepository.AddOfficeSubscription(ClientID, SubID, UsersPerSub);
        }


        public bool Exists(Client client)
        {
            return clientsRepository.CheckIfExists(client);
        }

        public bool ExistsAndDeleted(Client client)
        {
            return clientsRepository.CheckIfExistsAndDeleted(client);
        }


        public Client GetDeletedClient(Client client)
        {
            return clientsRepository.GetDeletedClient(client);
        }

        public bool NeedsRenewal(Client client)
        {
            return clientsRepository.NeedsRenewal(client);
        }


        public ClientSubscriptions GetClientSubscription(Guid ClientID, Guid SubID)
        {
            return clientsRepository.GetClientSubscription(ClientID, SubID);
        }


        public bool ClientNameAndMailExists(string ClientName, string ContactMail)
        {
            return clientsRepository.ClientNameAndMailExists(ClientName, ContactMail);
        }


        public bool ClientNameAndMailExistsAndDeleted(string ClientName, string ContactMail)
        {
            return clientsRepository.ClientNameAndMailExistsAndDeleted(ClientName, ContactMail);
        }
        public Client GetClientByNameAndMail(string ClientName,string ContactMail)
        {
            return clientsRepository.GetByNameAndMail(ClientName, ContactMail);
        }


        public IEnumerable<Client> GetClients()
        {
            return clientsRepository.GetMany(c => c.IsDeleted == false);
        }
    }
}
