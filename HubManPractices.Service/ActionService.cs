using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubManPractices.Models;
using HubManPractices.Repository.Repositories;
using HubManPractices.Repository;

namespace HubManPractices.Service
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository ActionsRepsoitory;

        private readonly ApplicationEntities ApplicationEntity;
        public ActionService(ApplicationEntities Ae, IActionRepository AR)
        {
            this.ActionsRepsoitory = AR;
            this.ApplicationEntity = Ae;
        }
        public void CreateAction(Models.Action Action)
        {
            ActionsRepsoitory.Add(Action);
        }

        public HubManPractices.Models.Action GetById(Guid id)
        {
            return ActionsRepsoitory.GetById(id);
        }

        public IEnumerable<HubManPractices.Models.Action> GetActions()
        {
            return ActionsRepsoitory.GetAll();
        }
    }
}
