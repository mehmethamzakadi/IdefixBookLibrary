using IdefixKitapLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace IdefixKitapLibrary.Database
{
    public class BookLibraryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=vt1.bam.belsis.dmz;Database=BookLibrary;User Id=postgres;Password=ada11sql;");
        }

        public DbSet<Kitap> Kitaplar { get; set; }
    }
}
