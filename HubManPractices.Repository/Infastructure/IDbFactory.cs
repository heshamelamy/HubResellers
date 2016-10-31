using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Infastructure
{
    public interface IDbFactory : IDisposable
    {
        ApplicationEntities Init();
    }
}
