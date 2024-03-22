using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options) { }
        public DbSet<KeyPair> KeyPairs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KeyPair>().HasKey(kp => kp.key);
        }
    }
}
