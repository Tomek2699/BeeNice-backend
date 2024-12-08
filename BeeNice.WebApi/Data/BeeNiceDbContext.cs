using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Data
{
    public class BeeNiceDbContext : IdentityDbContext
    {
        public BeeNiceDbContext(DbContextOptions<BeeNiceDbContext> options) : base(options)
        {

        }

        public DbSet<Apiary> Apiary { get; set; }
        public DbSet<Hive> Hive { get; set; }
        public DbSet<BeeFamily> BeeFamily { get; set; }
        public DbSet<Queen> Queen { get; set; }
        public DbSet<HoneyCollection> HoneyCollection { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<TherapeuticTreatment> TherapeuticTreatment { get; set; }
    }
}
