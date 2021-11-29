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
    public class BultoStorageService
    {

        //private IConfiguration _configuration;


        public BultoStorageService()
        {
            //_configuration = configuration;
        }


        public void SaveBultos(BultoProcesado bultoProcesado)
        {

            using (SqlConnection conn = new SqlConnection(@"Data Source=MD2V0GMC;Initial Catalog=MCGA.TpGrupal;Integrated Security=True"))
            {

                conn.Open();


                string sql = "INSERT INTO [AlmacenBultos] ([Id],[Peso],[Nombre]) VALUES(@Id,@Peso ,@Nombre)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = bultoProcesado.ID;
                    //cmd.Parameters.Add("@Peso", SqlDbType.Int).Value = bultoProcesado.Peso;

                    cmd.Parameters.Add(new SqlParameter("@Id", bultoProcesado.ID));
                    cmd.Parameters.Add(new SqlParameter("@Peso", bultoProcesado.Peso));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", bultoProcesado.Nombre));

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


            using (SqlConnection conn = new SqlConnection(@"Data Source=MD2V0GMC;Initial Catalog=MCGA.TpGrupal;Integrated Security=True"))
            {

                conn.Open();


                string sql = @"SELECT fecha_ingreso FROM [AlmacenBultos]";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    da.SelectCommand = cmd;
                    da.Fill(data);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    foreach (DataRow item in data.Rows)
                    {
                        bultoIngresado.Fecha = Convert.ToDateTime(item["fecha_ingreso"]);


                        dateTimes.Add(new PackageItem() { CreationDate = bultoIngresado.Fecha });
                    }



                    return dateTimes;
                }
            }
        }
    }
}
