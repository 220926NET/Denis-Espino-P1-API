namespace TicketSystem;
using Model;
using Database;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Serilog;

class TicketRepo
{
    public TicketRepo() {}
    public bool CreateTicket(int openedById, decimal amount, string description)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "INSERT INTO Tickets (OpenedById, Amount, Description, Status) VALUES (@openedById, @amount, @description, @status)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@openedById", openedById));
            command.Parameters.Add(new SqlParameter("@amount", amount));
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.Add(new SqlParameter("@status", Ticket.StatusEnum.Pending));
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return (rowsAffected == 1);
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.CreateTicket({openedById}, {amount}, {description})", openedById, amount, description);
                return false;
            }
        }
    }

    public bool ApproveTicket(int ticketId, int closedById)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "UPDATE Tickets SET Status = @status, ClosedById = @closedById WHERE Id = @id;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            command.Parameters.AddWithValue("@closedById", closedById);
            command.Parameters.AddWithValue("@status", Ticket.StatusEnum.Accepted);
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return (rowsAffected == 1);
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.ApproveTicket({ticketId}, {closedById})", ticketId, closedById);
                return false;
            }
        }
    }

    public bool DenyTicket(int ticketId, int closedById)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = "UPDATE Tickets SET Status = @status, ClosedById = @closedById WHERE Id = @id;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            command.Parameters.AddWithValue("@closedById", closedById);
            command.Parameters.AddWithValue("@status", Ticket.StatusEnum.Denied);
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return (rowsAffected == 1);
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.DenyTicket({ticketId}, {closedById})", ticketId, closedById);
                return false;
            }
        }
    }

    private Ticket ReadTicket(SqlDataReader reader)
    {
        int id = (int)reader["Id"];
        int openedById = (int)reader["OpenedById"];
        int closedById = (reader["ClosedById"] is DBNull) ? 0 : (int)reader["ClosedById"];
        decimal amount = (decimal)reader["Amount"];
        string description = (string)reader["Description"];
        Ticket.StatusEnum status = (Ticket.StatusEnum)reader["Status"];
        string? openedByUsername = (reader["OpenedByUsername"] is DBNull) ? null : (string)reader["OpenedByUsername"];
        string? closedByUsername = (reader["ClosedByUsername"] is DBNull) ? null : (string)reader["ClosedByUsername"];
        return new Ticket( id, openedById, closedById, amount, description, status, openedByUsername, closedByUsername);
    }

    private string SelectQuery(string? whereClause = null)
    {
        StringBuilder builder = new StringBuilder(
@"SELECT 
    T1.Id,
    T1.OpenedById,
    T1.OpenedByUsername,
    T2.ClosedById,
    T2.ClosedByUsername,
    T1.[Description],
    T1.Amount,
    T1.[Status]
FROM
    (SELECT 
        Tickets.Id As Id,
        Tickets.OpenedById As OpenedById,
        Employees.Username As OpenedByUsername,
        Tickets.Description As [Description],
        Tickets.Amount As Amount,
        Tickets.[Status] As [Status]
    FROM 
        Employees
    JOIN 
        Tickets 
    ON
        Tickets.OpenedById = Employees.Id) AS T1
LEFT JOIN
    (SELECT
        Tickets.Id As Id,
        Tickets.ClosedById As ClosedById,
        Employees.Username As ClosedByUsername
    FROM 
        Tickets
    JOIN 
        Employees 
    ON
        Tickets.ClosedById = Employees.Id) AS T2
ON
    T1.Id = T2.Id");

        if (whereClause is null)
            builder.Append(";");
        else
            builder.Append(whereClause);

        return builder.ToString();
    }
    public Ticket? GetTicketById(int ticketId)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = SelectQuery(" WHERE T1.Id = @id;");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", ticketId);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return ReadTicket(reader);
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.GetTicketById({ticketId})", ticketId);
                return null;
            }
        }
    }

    public List<Ticket> GetTicketsByEmployee(int employeeId)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = SelectQuery(" WHERE OpenedById = @openedById;");
            SqlCommand command = new SqlCommand(query, connection);
            List<Ticket> list = new List<Ticket>();
            command.Parameters.AddWithValue("@openedById", employeeId);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(ReadTicket(reader));
                }

                return list;
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.GetTicketsByEmployee({employeeId})", employeeId);
                return new List<Ticket>();
            }
        }
    }

    public List<Ticket> GetTicketsByStatus(Ticket.StatusEnum status)
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = SelectQuery(" WHERE Status = @status;");
            SqlCommand command = new SqlCommand(query, connection);
            List<Ticket> list = new List<Ticket>();
            command.Parameters.AddWithValue("@status", status);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(ReadTicket(reader));
                }

                return list;
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.GetTicketsByStatus({status})", status);
                return new List<Ticket>();
            }
        }
    }

    public List<Ticket> GetAllTickets()
    {
        SqlConnection connection = ConnectionFactory.GetConnection();
        using (connection)
        {
            connection.Open();
            string query = SelectQuery();
            SqlCommand command = new SqlCommand(query, connection);
            List<Ticket> list = new List<Ticket>();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(ReadTicket(reader));
                }
                return list;
            }
            catch //(Exception exception)
            {
                //exceptionLogger.Error(exception, "TicketSystem.TicketRepo.GetAllTickets()");
                return new List<Ticket>();
            }
        }
    }

    
}