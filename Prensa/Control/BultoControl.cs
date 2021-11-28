using CommonDomain;
using Prensa.PrensaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prensa.Control
{
    public static class BultoControl
    {
        static BultoControl()
        {
            // TODO: hacer que bulto control guarde el bulto procesado cuando 

        }

        public static void LlevarBultoALaPila(BultoProcesado bulto)
        {
            PilaDeBultos.AgregarBulto(bulto);
        }
    }

    public static class PilaDeBultos
    {

        public static void AgregarBulto(BultoProcesado bulto)
        {
            CommonServices.Context.BultoStorageService.SaveBultos(bulto);
        }
    }
}
