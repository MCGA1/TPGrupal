using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Context
{
    public class BultosStorageContext : DbContext
    {

        public BultosStorageContext()
        {

        }
        public BultosStorageContext(DbContextOptions<BultosStorageContext> options)
         : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-JP7JEOE;Initial Catalog=BultosStorage;Integrated Security=True");
        }

        public DbSet<IBultoProcesado> BultoProcesados { get; set; }







    }
}