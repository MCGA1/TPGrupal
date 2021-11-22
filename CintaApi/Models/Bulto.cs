using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Models
{
    public class Bulto
    {
    
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        public int Peso { get; set; }

        public bool Preparado { get; set; }
    }
}