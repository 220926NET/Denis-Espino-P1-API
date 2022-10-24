using Model;
namespace TicketSystem;
public interface ITicketService
{
    public bool CreateTicket(int employeeId, decimal amount, string description);
    public bool CreateTicket(int employeeId, decimal amount, string description, out string message);
    public bool ApproveTicket(int employeeId, int ticketId);
    public bool ApproveTicket(int employeeId, int ticketId, out string message);
    public bool DenyTicket(int employeeId, int ticketId);
    public bool DenyTicket(int employeeId, int ticketId, out string message);
    public List<Ticket> GetTicketsByEmployee(int employeeId);
    public List<Ticket> GetAllTickets();
    public List<Ticket> GetTicketsByStatus(Ticket.StatusEnum status);
    public Ticket? GetTicketById(int ticketId);
}