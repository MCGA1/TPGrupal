using CintaApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Interfaces
{
    public interface IServiceBusQueueMessage
    {

        Task  PonerBulto(IEnumerable<Bulto> entity);

        Task PonerBulto(Bulto entity);

        Task<Bulto> GetIndididualBulto(string bultoId);

        Task<List<string>> CheckQueueMensagges();

        void PublicarBulto(object o, DoWorkEventArgs args);
    }
}
