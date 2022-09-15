using AuthenticationSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AuthenticationSystem.Quickstart
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<UserContext>();

            var connectionString = configuration.GetConnectionString("Users");

            builder.UseSqlServer(connectionString);

            return new UserContext(builder.Options);
        }
    }
}
