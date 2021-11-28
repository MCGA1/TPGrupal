using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public class BultoIngresado
    {
        [Key]
        public Guid Id { get; set; }

        public int Peso { get; set; }

        public string Nombre { get; set; }

        public bool Enviado { get; set; }
    }
}
