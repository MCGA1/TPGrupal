using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public static class BultoIngresadoService
    {
        private static BultoIngresadocContext _ingresadoContext;

        static  BultoIngresadoService()
        {
            _ingresadoContext = new BultoIngresadocContext();
        }


        public static void SaveBultoIngresado ()
        {
            var bulto = new BultoIngresado()
            {
                Id = new Guid(),
                Enviado = true,
                Nombre = "bulto demo",
                Peso = 100
            };


               

            _ingresadoContext.BultoIngresados.Add(bulto);
            _ingresadoContext.SaveChanges();

        }


    }
}
