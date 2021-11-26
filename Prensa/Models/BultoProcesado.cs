using CommonServices.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.Control
{
    public class BultoProcesado : IBultoProcesado
    {
        public Guid Id { get;}

        public int Peso { get;}

        public string Nombre { get; set; }

        public BultoProcesado(Bulto bulto)
        {
            Id = bulto.Id;
            Peso = bulto.Peso;
            Nombre = bulto.Nombre;
        }
    }
}
