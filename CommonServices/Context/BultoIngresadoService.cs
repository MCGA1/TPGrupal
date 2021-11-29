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


                string sql = "INSERT INTO [BultoIngresado] ([Id],[Enviado],[Peso],[Nombre],[Fecha]) VALUES(@Id,@Enviado,@Peso,@Nombre,@Fecha)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = bultoIngresados.Id;
                    cmd.Parameters.Add("@Enviado", SqlDbType.Bit).Value = bultoIngresados.Enviado;
                    cmd.Parameters.Add("@Peso", SqlDbType.Int).Value = bultoIngresados.Peso;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = bultoIngresados.Nombre;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = bultoIngresados.Fecha;


                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public static void UpdateBultoIngresado(Guid Id, DateTime Fecha)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source =localhost; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = "UPDATE  [BultoIngresado] SET Enviado=1 AND Fecha=@Fecha WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Fecha;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static object GetDateTimes()
        {
            var data = new DataTable();
            var bultoIngresado = new BultoIngresado();

            List<DateTime> dateTimes = new List<DateTime>();
            SqlDataAdapter da = new SqlDataAdapter();


            using (SqlConnection conn = new SqlConnection(@"Data Source =localhost; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = @"SELECT Fecha FROM   [BultoIngresado]";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    da.SelectCommand = cmd;
                    da.Fill(data);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    foreach (DataRow item in data.Rows)
                    {
                        bultoIngresado.Fecha = Convert.ToDateTime(item["Fecha"]);


                        dateTimes.Add(bultoIngresado.Fecha);
                    }



                    return dateTimes;
                }
            }
        }
    }
}