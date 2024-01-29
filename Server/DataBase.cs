using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    public class DataBase:DbContext
    {
        public static string sqlstr;
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(sqlstr);
        }
        public void AddProduct()
        {

        }
        public void AddUser()
        {

        }
        static public void ShowTables()
        {
            List<string> GetTables(string connectionString)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    List<string> TableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        TableNames.Add(row[2].ToString());
                    }
                    return TableNames;
                }
            }
            List<string> tables = GetTables(sqlstr);
            foreach(var table in tables)
            {
                Console.WriteLine(table);
            }
        }
        static public void PrintTable()
        {
            Console.WriteLine("\nSelect table:");
            List<string> GetTables(string connectionString)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    List<string> TableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        TableNames.Add(row[2].ToString());
                    }
                    return TableNames;
                }
            }
            List<string> tables = GetTables(sqlstr);
            for(int i = 0; i < tables.Count; i++)
            {
                Console.WriteLine($"{i} - {tables[i]}");
            }
            int.TryParse(Console.ReadLine(), out int tableId);
            if(tableId > tables.Count) Console.WriteLine("Error index above array size!");
            
        }
    }
}
