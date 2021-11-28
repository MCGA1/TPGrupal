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


        public static void SaveBultoIngresado (BultoIngresado bultoIngresados)
        {
         
            _ingresadoContext.BultoIngresados.Add(bultoIngresados);
            _ingresadoContext.SaveChanges();

        }

        public static void  UpdateBultoIngresado(Guid Id)
        {
            var result = _ingresadoContext.BultoIngresados.FirstOrDefault(x => x.Id == Id);

            result.Enviado = true;

            _ingresadoContext.BultoIngresados.Update(result);
            _ingresadoContext.SaveChanges();

        }


    }
}
