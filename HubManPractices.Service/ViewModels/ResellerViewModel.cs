using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service.ViewModels
{
   public  class ResellerViewModel
    {
        ResellerViewModel()
        {
            Clients = new List<Client>();
        }
        public Guid ResellerID { get; set; }
        public string Name { get; set; }
        public virtual string ResellerImage { get; set; }
        public int ClientsQuota { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}
