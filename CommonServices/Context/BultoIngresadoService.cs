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
    public static class BultoIngresadoService
    {


        static BultoIngresadoService()
        {
        }


        public static void SaveBultoIngresado(BultoIngresado bultoIngresados)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP - JP7JEOE; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = "INSERT INTO [BultoIngresado] ([Id],[Enviado],[Peso],[Nombre]) VALUES(@Id,@Enviado,@Peso,@Nombre)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = bultoIngresados.Id;
                    cmd.Parameters.Add("@Enviado", SqlDbType.Bit).Value = bultoIngresados.Enviado;
                    cmd.Parameters.Add("@Peso", SqlDbType.Int).Value = bultoIngresados.Peso;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = bultoIngresados.Nombre;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public static void UpdateBultoIngresado(Guid Id)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP - JP7JEOE; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = "UPDATE  [BultoIngresado] SET Enviado=1 WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}