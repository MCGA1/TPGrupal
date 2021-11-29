using CommonServices.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices
{
    public static class BultoManagementFactory
    {

        public static BultoIngresadoService GetBultoIngresadoManager()
        {
            return new BultoIngresadoService();
        }

        public static BultoStorageService GetBultoStorageService()
        {
            return new BultoStorageService();
        }

        static BultoManagementFactory()
        {

        }

    }
}
