using Microsoft.EntityFrameworkCore;
using AuthenticationSystem.Data.Models;

namespace AuthenticationSystem.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<Models.Users> Users { get; set; }
    }
}
