using Microsoft.Data.SqlClient;
using SQLDatabaseApp;

string connectionString = "Server=localhost;Database=AcademyDB;User Id=sa;Password=yourStrong(!)Password;Encrypt=False";

//Task 1.1
try
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connected successfully!");
    }
}
catch (SqlException ex)
{
    Console.WriteLine("Connection failed(");
    Console.WriteLine($"Error: {ex.Message}");
}
//Task 1.2
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    SqlCommand command = new SqlCommand("SELECT COUNT(Id) FROM Users", connection);
    int usersCount = Convert.ToInt32(command.ExecuteScalar());
    Console.WriteLine($"Count of users: {usersCount}");
}


//Task 1.3
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    string sql = "SELECT * FROM Products WHERE Name = @userInput";
    SqlCommand command = new SqlCommand(sql, connection);

    string userInput = Console.ReadLine();
    command.Parameters.AddWithValue("@userInput", userInput);
}

//Task 2.1 
List<Product> GetAllProducts(string connectionString)
{
    List<Product> products = new List<Product>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string sql = "SELECT Id, Name, Price, IsAvailable FROM Products";

        using (SqlCommand command = new SqlCommand(sql, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Price = (decimal)reader["Price"],
                    IsAvailable = (bool)reader["IsAvailable"]
                };

                products.Add(product);
            }
        }
    }
    return products;
}

//Task 2.2, 2.3
List<Product> GetProductsByPrice(string connectionString, decimal maxPrice)
{
    List<Product> products = new List<Product>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string sql = "SELECT Id, Name, Price, IsAvailable FROM Products WHERE Price <= @maxPrice";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            // 2.3 (
            command.Parameters.AddWithValue("@maxPrice", maxPrice);
            // )
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int idIndex = reader.GetOrdinal("Id");
                int nameIndex = reader.GetOrdinal("Name");
                int priceIndex = reader.GetOrdinal("Price");
                int availableIndex = reader.GetOrdinal("IsAvailable");

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32(idIndex),
                        Name = reader.GetString(nameIndex),
                        Price = reader.GetDecimal(priceIndex),
                        IsAvailable = reader.GetBoolean(availableIndex)
                    };

                    products.Add(product);
                }
            }
        }
    }
    return products;
}

//Task 3
partial class Program
{
    static string connectionString = "Server=localhost;Database=TodoDb;User Id=sa;Password=yourStrong(!)Password;Encrypt=False";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add task");
            Console.WriteLine("2. Show tasks");
            Console.WriteLine("3. Mark task as done");
            Console.WriteLine("4. Delete all tasks");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTodo();
                    break;
                case "2":
                    ShowTodos();
                    break;
                case "3":
                    MarkDone();
                    break;
                case "4":
                    DeleteAll();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Wrong choice!");
                    break;
            }
        }
    }

    static void AddTodo()
    {
        Console.Write("Enter task title: ");
        string? title = Console.ReadLine();

        string sql = "INSERT INTO Todos (Title, IsDone) OUTPUT INSERTED.Id VALUES (@title, 0)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(sql, connection))
        {
            cmd.Parameters.AddWithValue("@title", title);
            connection.Open();
            int newId = (int)cmd.ExecuteScalar();
            Console.WriteLine($"Id: {newId}");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine("Done: No");
        }
    }

    static void ShowTodos()
    {
        string sql = "SELECT Id, Title, IsDone FROM Todos";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(sql, connection))
        {
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(reader.GetOrdinal("Id"))}");
                    Console.WriteLine($"Title: {reader.GetString(reader.GetOrdinal("Title"))}");
                    Console.WriteLine($"Done: {(reader.GetBoolean(reader.GetOrdinal("IsDone")) ? "Yes" : "No")}");
                    Console.WriteLine();
                }
            }
        }
    }

    static void MarkDone()
    {
        Console.Write("Enter Id of task to mark done: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        string sql = "UPDATE Todos SET IsDone = 1 WHERE Id = @id";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(sql, connection))
        {
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Task marked as done." : "Task not found.");
        }
    }

    static void DeleteAll()
    {
        string sql = "DELETE FROM Todos";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(sql, connection))
        {
            connection.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Deleted {rows} tasks.");
        }
    }
}