using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerACount.Data.Entities;

namespace ManagerACount.Data
{
    public partial class EfManagerAccountConfigurationsContext: DbContext
    {
        public EfManagerAccountConfigurationsContext(DbContextOptions<EfManagerAccountConfigurationsContext> options)
: base(options)
        { }

        public virtual DbSet<LogSession> LogSession { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:study-friends.database.windows.net,1433;Initial Catalog=ManagerAccount;Persist Security Info=False;User ID=manager;Password=72jeQYB*58cQ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });

                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Configurations");
        }
    }
}
