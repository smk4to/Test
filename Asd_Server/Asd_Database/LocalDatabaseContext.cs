using Asd_Configuration;
using Asd_Models;
using Microsoft.EntityFrameworkCore;

namespace Asd_Database
{
    public class LocalDatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasMany(i => i.Employees).WithOne(i => i.Department).HasForeignKey(i => i.DepartmentId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                switch (Configuration.LocalDatabaseSystem())
                {
                    case Asd_E_Database_System.Postgres:
                        optionsBuilder.UseNpgsql(Configuration.LocalConnectionString());
                        break;
                    case Asd_E_Database_System.MsSql:
                        optionsBuilder.UseSqlServer(Configuration.LocalConnectionString());
                        break;
                }
            }
            base.OnConfiguring(optionsBuilder);
            //TODO: optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
