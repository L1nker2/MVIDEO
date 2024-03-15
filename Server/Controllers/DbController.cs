using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.Models;
using System.Data;

namespace Server.Controllers
{
    internal class DbController : DataBase
    {
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

            foreach (var table in tables)
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
                                Console.WriteLine("\n"+reader[i]);
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

            var temp = new Dictionary<string, string>();

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

            using (SqlConnection connection = new SqlConnection(sqlstr))
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


        static public int Registration(string fname, string sname, string login, string pass)
        {
            try
            {
                using (DataBase db = new())
                {
                    User user = new()
                    {
                        FName = fname,
                        SName = sname,
                        Login = login,
                        Password = pass
                    };
                    db.Users.Add(user);
                    db.SaveChanges();

                    User user_return = db.Users.FirstOrDefault(el => el.FName == fname || el.SName == sname);
                    return user_return.Id;
                }
            }
            catch (Exception ex)
            {
                // Проверьте внутреннее исключение для получения подробной информации об ошибке
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
                return -1; // или выберите другое значение для обработки ошибки
            }
        }


        static public User Login(string login, string pass)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", pass);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Проверяем, не являются ли значения полей NULL
                            int userId = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id"));
                            string fName = reader.IsDBNull(reader.GetOrdinal("FName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FName"));
                            string sName = reader.IsDBNull(reader.GetOrdinal("SName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SName"));
                            string basket = reader.IsDBNull(reader.GetOrdinal("Basket")) ? string.Empty : reader.GetString(reader.GetOrdinal("Basket"));

                            // Создаем экземпляр User и устанавливаем значения полей
                            user = new User { Id = userId, FName = fName, SName = sName, Login = login, Password = pass, Basket = basket };
                        }
                    }
                }
            }

            return user;
        }


        public static List<Product> Basket(int id)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();

                string selectQuery = "SELECT Basket FROM Users WHERE Id = @Id";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", id);

                    string basket = (string)selectCommand.ExecuteScalar();

                    if (string.IsNullOrEmpty(basket))
                    {
                        return null;
                    }

                    var tovarsId = basket.Split(";");

                    foreach (var tovarId in tovarsId)
                    {
                        string selectProductQuery = "SELECT * FROM Product WHERE Id = @TovarId";

                        using (SqlCommand selectProductCommand = new SqlCommand(selectProductQuery, connection))
                        {
                            selectProductCommand.Parameters.AddWithValue("@TovarId", int.Parse(tovarId));

                            using (SqlDataReader reader = selectProductCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Product product = new Product
                                    {
                                        Id = (int)reader["Id"],
                                        Name = (string)reader["Name"],
                                        Price = (string)reader["Price"],
                                        ImgBase64 = (string)reader["ImgBase64"],
                                        Count = (int)reader["Count"],
                                        Description = (string)reader["Description"],
                                        Category = (string)reader["Category"]
                                    };

                                    products.Add(product);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
            }

            return products;
        }


        static public void AddToBasket(string userId, string productId)
        {
            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();

                // Получить текущее значение поля Basket по указанному UserId
                string selectQuery = "SELECT Basket FROM Users WHERE Id = @UserId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@UserId", userId);
                    string currentBasket = (string)selectCommand.ExecuteScalar();

                    // Обновить поле Basket, добавив новый айди товара
                    string updatedBasket = string.IsNullOrEmpty(currentBasket) ? productId.ToString() : currentBasket + ";" + productId;
                    string updateQuery = "UPDATE Users SET Basket = @Basket WHERE Id = @UserId";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Basket", updatedBasket);
                        updateCommand.Parameters.AddWithValue("@UserId", userId);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
        }


        static public void RemoveProduct(int userId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(sqlstr))
            {
                connection.Open();

                // Получить текущее значение поля bucket по указанному userId
                string selectQuery = "SELECT basket FROM Users WHERE Id = @userId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@userId", userId);
                    string currentBasket = (string)selectCommand.ExecuteScalar();

                    // Разбить значение поля bucket на отдельные идентификаторы товаров
                    List<string> basketItems = currentBasket.Split(';').ToList();

                    // Удалить указанный productId из списка товаров
                    basketItems.Remove(productId.ToString());

                    // Обновить поле bucket
                    string updatedBasket = string.Join(";", basketItems);
                    string updateQuery = "UPDATE Users SET basket = @updatedBasket WHERE Id = @userId";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@updatedBasket", updatedBasket);
                        updateCommand.Parameters.AddWithValue("@userId", userId);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
