using CintaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Interfaces
{
    public interface IServiceBusQueueMessage
    {
        void PonerBulto(Bulto entity);

        Task<IEnumerable<Bulto>> SacarBulto(string id);
    }
}
