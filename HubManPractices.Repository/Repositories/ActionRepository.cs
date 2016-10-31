using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public class ActionRepository : RepositoryBase<HubManPractices.Models.Action>, IActionRepository
    {
        public ActionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
