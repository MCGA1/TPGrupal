using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain
{
    public class BultoProcesado
    {
        [Key]
        public Guid ID { get; set; }

        public int Peso { get; set; }

        public string Nombre { get; set; }

        public DateTime Fecha { get; set; }



        public BultoProcesado(Bulto bulto)
        {
            ID = bulto.Id;
            Peso = bulto.Peso;
            Nombre = bulto.Nombre;
            Fecha = bulto.Fecha;
        }

        public BultoProcesado()
        {

        }
    }


}
