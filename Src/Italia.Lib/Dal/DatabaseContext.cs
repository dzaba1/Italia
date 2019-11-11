using Italia.Lib.Model;
using Microsoft.EntityFrameworkCore;

namespace Italia.Lib.Dal
{
    internal sealed class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }
    }
}
