using Model;
using Database;
using Microsoft.Data.SqlClient;
using Serilog;

namespace Auth;

internal class AuthRepo
{
    public bool CreateEmployee(string username, string password, Employee.RoleEnum role)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "INSERT INTO Employees (Username, Password, Role) VALUES (@username, @password, @role)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.Add(new SqlParameter("@role", role));
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return (rowsAffected == 1);
            }
            catch //(Exception exception)
            {
                return false;
            }
        }
    }

    public Employee? GetEmployeeByUsername(string username)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "SELECT * FROM Employees WHERE Username = @username;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return new Employee(
                    (int)reader["Id"],
                    (string)reader["Username"],
                    (string)reader["Password"],
                    (Employee.RoleEnum)reader["Role"]);
            }
            catch //(Exception exception)
            {
                return null;
            }
        }
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "SELECT * FROM Employees WHERE Id = @id;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", employeeId);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return new Employee(
                    (int)reader["Id"],
                    (string)reader["Username"],
                    (string)reader["Password"],
                    (Employee.RoleEnum)reader["Role"]);
            }
            catch //(Exception exception)
            {
                return null;
            }
        }
    }
}