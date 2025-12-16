using APIProjeKampi_WepApi.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using Image = APIProjeKampi_WepApi.Entities.Image;

namespace APIProjeKampi_WepApi.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=YummyDb;Integrated Security=True;" +
                "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;" +
                "Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=30");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Contact> Contacts{ get; set; }
        public DbSet<Feature> Features{ get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages{ get; set; }

        public DbSet<Product> Products{ get; set; }
        public DbSet<Reservation> Reservations{ get; set; }
        public DbSet<Service> Services{ get; set; }
        public DbSet<Testiomanial> Testiomanials{ get; set; }
        public DbSet<YummyEvent> YummyEvents{ get; set; }
        public DbSet<Notification> Notifications{ get; set; }
        public DbSet<About> Abouts{ get; set; }






    }
}