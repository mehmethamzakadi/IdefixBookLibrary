using IdefixKitapLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace IdefixKitapLibrary.Database
{
    public class BookLibraryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Database=BookLibrary;User Id=postgres;Password=postgrespw;");
        }

        public DbSet<Kitap> Kitaplar { get; set; }
    }
}
