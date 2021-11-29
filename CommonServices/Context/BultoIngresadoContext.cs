//using CommonDomain;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CommonServices.Context
//{
//    public   class BultoIngresadocContext : DbContext
//    {

//        public BultoIngresadocContext()
//        {

//        }
//        public BultoIngresadocContext(DbContextOptions<BultosStorageContext> options)
//         : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<BultoIngresado>()
//                .Property(b => b.Id)
//                .IsRequired();

//            modelBuilder.Entity<BultoIngresado>()
//            .Property(c => c.RowVersion)
//            .IsRowVersion();

//            modelBuilder.Entity<BultoIngresado>().ToTable("BultoIngresado");

//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-JJADIK1;Initial Catalog=RegistroBultoIngresado;Integrated Security=True");
//        }


//        public DbSet<BultoIngresado> BultoIngresados { get; set; }






//    }
//}