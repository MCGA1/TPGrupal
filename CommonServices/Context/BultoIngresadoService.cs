using CommonDomain;
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
    public class BultoIngresadoService
    {


        public BultoIngresadoService()
        {
        }


        public void SaveBultoIngresado(BultoIngresado bultoIngresados)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source = localhost; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = "INSERT INTO [BultoIngresado] ([Id],[Enviado],[Peso],[Nombre]) VALUES(@Id,@Enviado,@Peso,@Nombre)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = bultoIngresados.Id;
                    //cmd.Parameters.Add("@Enviado", SqlDbType.Bit).Value = bultoIngresados.Enviado;
                    //cmd.Parameters.Add("@Peso", SqlDbType.Int).Value = bultoIngresados.Peso;
                    //cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = bultoIngresados.Nombre;


                    cmd.Parameters.Add(new SqlParameter("@Id", bultoIngresados.Id));
                    cmd.Parameters.Add(new SqlParameter("@Enviado", bultoIngresados.Enviado));
                    cmd.Parameters.Add(new SqlParameter("@Peso", bultoIngresados.Peso));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", bultoIngresados.Nombre));

                    //cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = bultoIngresados.Fecha;


                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public void UpdateBultoIngresado(Guid Id)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source = localhost; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = "UPDATE  [BultoIngresado] SET Enviado=1  WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PackageItem> GetDateTimes()
        {
            var data = new DataTable();
            var bultoIngresado = new BultoIngresado();

            List<PackageItem> dateTimes = new List<PackageItem>();
            SqlDataAdapter da = new SqlDataAdapter();


            using (SqlConnection conn = new SqlConnection(@"Data Source =localhost; Initial Catalog = RegistroBultoIngresado; Integrated Security = True"))
            {

                conn.Open();


                string sql = @"SELECT Fecha FROM [BultoIngresado]";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    da.SelectCommand = cmd;
                    da.Fill(data);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    foreach (DataRow item in data.Rows)
                    {
                        bultoIngresado.Fecha = Convert.ToDateTime(item["Fecha"]);


                        dateTimes.Add(new PackageItem() { CreationDate = bultoIngresado.Fecha });
                    }



                    return dateTimes;
                }
            }
        }
    }
}