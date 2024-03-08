using Microsoft.EntityFrameworkCore;
using Server.Properties;

namespace Server.Models
{
    public class DataBase : DbContext
    {
        public static string sqlstr = Resources.sqlstr;
        public DbSet<Product> Product { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(sqlstr);
            
        }
    }
}
