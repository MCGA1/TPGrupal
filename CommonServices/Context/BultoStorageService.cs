using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public static class BultoStorageService
    {
        private static  BultosStorageContext _bultosStorageContext;


         static BultoStorageService()
        {
            _bultosStorageContext = new BultosStorageContext();
        }


        public  static void SaveBultos(IBultoProcesado bultoProcesado)
        {
            _bultosStorageContext.BultoProcesados.Add(bultoProcesado);
            _bultosStorageContext.SaveChangesAsync();
        }


    }

}
