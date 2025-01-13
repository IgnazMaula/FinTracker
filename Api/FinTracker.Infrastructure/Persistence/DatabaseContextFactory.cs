using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FinTracker.Infrastructure.Persistence;

namespace FinTracker.Infrastructure.Persistence
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            string connectionString = "Server=IGNAZ-LAPTOP\\SQLEXPRESS;Database=FinTracker_DB;Integrated Security=True;TrustServerCertificate=True;";
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}