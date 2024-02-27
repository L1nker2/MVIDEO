using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Server
{
    public class DataBase:DbContext
    {
        public static string sqlstr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\MVIDEO.mdf;Integrated Security=True;Connect Timeout=30";
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(sqlstr);
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
            Console.WriteLine("\nSelect table:\n");
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
            for (int i = 0; i < tables.Count; i++)
            {
                Console.WriteLine($"{i} - {tables[i]}");
            }
            int.TryParse(Console.ReadLine(), out int tableId);
            if (tableId > tables.Count) Console.WriteLine("\nError index above array size!\n");
            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();

                string query = $"SELECT * FROM {tables[tableId]}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader[i]);
                            }
                        }
                    }
                }
            }
        }
        static public void InsertLine()
        {
            Console.WriteLine("\nSelect table:\n");
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
            List<string> columns = new List<string>();
            for (int i = 0; i < tables.Count; i++)
            {
                Console.WriteLine($"{i} - {tables[i]}");
            }
            int.TryParse(Console.ReadLine(), out int tableId);
            if (tableId > tables.Count) Console.WriteLine("\nError index above array size!\n");
            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();
                string query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tables[tableId]);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            Console.WriteLine("Column Name: " + columnName + ", Data Type: " + dataType + "\n");
                            if (columnName != "Id") columns.Add(columnName);
                        }
                    }
                }
            }
            if (tables[tableId] == "Product")
            {
                Console.WriteLine("\nInstead of the value for \"ImgBase64\" just enter the path for the image!\n");
            }
            var temp = new Dictionary<string, string> ();
            foreach (var column in columns)
            {
                Console.WriteLine($"\nInsert value for \"{column}\":\n");
                string inp = Console.ReadLine();
                if (inp == "")
                {
                    Console.WriteLine("\nError null input\n");
                    return;
                }
                temp.Add(column, inp);
            }
            if (tables[tableId] == "Product")
            {
                temp["ImgBase64"] = Convert.ToBase64String(File.ReadAllBytes(temp["ImgBase64"]));
            }
            using(SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();
                string fields = string.Join(", ", temp.Keys);
                string values = string.Join(", ", temp.Values.Select(value => $"'{value}'"));
                string query = $"INSERT INTO {tables[tableId]} ({string.Join(",", temp.Keys)}) " +
                                           $"VALUES (N'{string.Join("', N'", temp.Values)}')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("\nThe addition was successful, you can check it\n");
        }
    }
}
