using System.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

string connectionString =
    "Server=localhost;Database=AdoNetAdvancedDB;User Id=sa;Password=yourStrong(!)Password;Encrypt=False;";
//Task 1.1

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    bool Login(SqlConnection conn, string username, string password)
    {
        string sql = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
        using SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;

        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }

    bool isLogined = Login(connection, "admin", "supersecret");
    Console.WriteLine(isLogined);
}

//Task 1.2
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    void PrintProducts(SqlDataReader reader)
    {
        int descOrdinal = reader.GetOrdinal("Description");
        while (reader.Read())
        {
            if (reader.IsDBNull(descOrdinal))
            {
                Console.WriteLine("There is no description provided for this product.");
            }
            else
            {
                string description = reader.GetString(descOrdinal);
                Console.WriteLine($"Опис: {description}");
            }
        }
    }

    SqlCommand command = new SqlCommand("SELECT Description FROM Products", connection);
    SqlDataReader reader = command.ExecuteReader();
    PrintProducts(reader);
}

//task 2.1

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    void TransferMoney(SqlConnection conn, int fromAccount, int toAccount, decimal amount)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            using SqlCommand cmdWithdraw =
                new SqlCommand("UPDATE Accounts SET Balance = Balance - @amount WHERE Id = @from", conn, transaction);
            // 2. ПРИВ'ЯЖІТЬ ТРАНЗАКЦІЮ ДО КОМАНДИ
            cmdWithdraw.Parameters.AddWithValue("@amount", amount);
            cmdWithdraw.Parameters.AddWithValue("@from", fromAccount);
            cmdWithdraw.ExecuteNonQuery();

            // Імітація збою мережі: throw new Exception("Мережа впала!");
            throw new Exception("Мережа впала!");

            using SqlCommand cmdDeposit = new SqlCommand("UPDATE Accounts SET Balance = Balance + @amount WHERE Id = @to", conn, transaction);
            cmdDeposit.Parameters.AddWithValue("@amount", amount);
            cmdDeposit.Parameters.AddWithValue("@to", toAccount);
            cmdDeposit.ExecuteNonQuery();

            // 3. ЗРОБІТЬ COMMIT ТУТ
            transaction.Commit();
            Console.WriteLine("Переказ успішний!");
        }
        catch (Exception ex)
        {
            // 4. ЗРОБІТЬ ROLLBACK ТУТ
            transaction.Rollback();
            Console.WriteLine($"Помилка: {ex.Message}. Гроші повернуто.");
        }
    }

    TransferMoney(connection, 1, 2, 100);
}

//Task 2.2
void ConnectToDatabase(string connectionString)
{
    // ЗАМІНІТЬ ЦІ РЯДКИ НА ВИКОРИСТАННЯ ФАБРИКИ (DbProviderFactory):
    DbProviderFactory factory = SqlClientFactory.Instance;

    using DbConnection conn = factory.CreateConnection();
    conn.ConnectionString = connectionString;
    conn.Open();

    using DbCommand cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT GETDATE()";

    var result = cmd.ExecuteScalar();
    Console.WriteLine($"Час на сервері: {result}");
}

ConnectToDatabase(connectionString);

//Task 3
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    void PlaceOrder(string connectionString, int productId, int quantity, string customerName)
    {
        using SqlConnection conn = new SqlConnection(connectionString);
        conn.Open();

        // ВАШ КОД ТУТ (Транзакція -> Перевірка -> Insert -> Update -> Commit/Rollback)
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            string sql = "SELECT Stock FROM Product WHERE Id = @productId";
            using SqlCommand checkCmd = new SqlCommand(sql, conn, transaction);
            checkCmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;

            int stock;
            using (SqlDataReader reader = checkCmd.ExecuteReader())
            {
                if (reader.Read())
                    stock = reader.GetInt32(0);
                else
                    throw new Exception("Can't find stock");
            }
            
            if (stock < quantity)
                throw new Exception("Not enough stock");
            
            string insertSql = @"INSERT INTO Orders (ProductId, CustomerName, Quantity, OrderDate) VALUES (@productId, @customerName, @quantity, @orderDate)";
            
            using SqlCommand insertCmd = new SqlCommand(insertSql, conn, transaction);
            insertCmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;
            insertCmd.Parameters.Add("@customerName", SqlDbType.NVarChar, 100).Value = customerName;
            insertCmd.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
            insertCmd.Parameters.Add("@orderDate", SqlDbType.DateTime).Value = DateTime.Now;
            
            insertCmd.ExecuteNonQuery();
            
            string updateSql = "UPDATE Product SET Stock = Stock - @quantity WHERE Id = @productId";
            using SqlCommand updateCmd = new SqlCommand(updateSql, conn, transaction);
            updateCmd.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
            updateCmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;
            
            updateCmd.ExecuteNonQuery();

            transaction.Commit();
            Console.WriteLine("Order Placed!");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error: {ex.Message}. Transaction canceled");
        }
    }
}