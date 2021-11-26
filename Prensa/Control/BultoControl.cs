using CommonServices.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.Control
{
    public static class BultoControl
    {

        public static void LlevarBultoALaPila(BultoProcesado bulto)
        {

            PilaDeBultos.AgregarBulto(bulto);
        }
    }

    public static class PilaDeBultos
    {

        public static void AgregarBulto(BultoProcesado bulto)
        {
            BultosStorageContext storageContext = new BultosStorageContext();
            storageContext.BultoProcesados.Add(bulto);

            storageContext.SaveChangesAsync();

            // TODO: Conexion a base de datos
        }
    }
}

