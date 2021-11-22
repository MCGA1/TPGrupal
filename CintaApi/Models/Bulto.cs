using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Models
{
    public class Bulto
    {

        private Guid id;


        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }

            set { id  = value; }
        }
    
        public string Nombre { get; set; }

        public int Peso { get; set; }
    }
}
