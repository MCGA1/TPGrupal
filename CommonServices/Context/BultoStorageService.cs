using CommonDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public  static class BultoStorageService
    {

        //private IConfiguration _configuration;


        static BultoStorageService()
        {
            //_configuration = configuration;
        }


        public static void SaveBultos(BultoProcesado bultoProcesado)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source = localhost; Initial Catalog = BultosStorage; Integrated Security = True"))
            {

                conn.Open();


                string sql = "INSERT INTO [BultosProcesados] ([Id],[Peso],[Nombre]) VALUES(@Id,@Peso ,@Nombre)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = bultoProcesado.ID;
                    cmd.Parameters.Add("@Peso", SqlDbType.Int).Value = bultoProcesado.Peso;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = bultoProcesado.Nombre;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}