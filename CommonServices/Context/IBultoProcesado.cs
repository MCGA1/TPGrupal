using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public interface IBultoProcesado
    {
        public Guid Id { get; }

        public int Peso { get; }

        public string Nombre { get; set; }
    }
}
