using finallab.Models;
using Microsoft.EntityFrameworkCore;
namespace finallab.Database
{
    public class DataDbContext : DbContext
    {
        //Constructure Method
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        // Tabke 
        public DbSet<employees> employees{ get; set; }

        // Tabke Manufacturers
        public DbSet<positions> positions { get; set; }
    }
}
