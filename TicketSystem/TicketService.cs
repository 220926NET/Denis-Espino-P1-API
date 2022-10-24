using Model;

namespace TicketSystem;
public class TicketService : ITicketService
{
    private TicketRepo repo;
    private IAuthService authService;
    
    public TicketService(IAuthService authService)
    {
        repo = new TicketRepo();
        this.authService = authService;
    }

    public bool CreateTicket(int employeeId, decimal amount, string description) => CreateTicket(employeeId, amount, description, out _);
    public bool CreateTicket(int employeeId, decimal amount, string description, out string message)
    {
        // Check employee existence before creating ticket
        if (authService.GetEmployeeById(employeeId) is null)
        {
            message = "Invalid employee credentials.";
            return false;
        }
        
        if (repo.CreateTicket(employeeId, amount, description))
        {
            message = "The ticket has been successfully created.";
            return true;
        }
        else
        {
            message = "The ticket was unable to be created.";
            return false;
        }
    }

    public bool ApproveTicket(int employeeId, int ticketId) => ApproveTicket(employeeId, ticketId, out _);
    public bool ApproveTicket(int employeeId, int ticketId, out string message)
    {
        // Check employee existence before creating ticket
        Employee? employee = authService.GetEmployeeById(employeeId);
        if (employee is null)
        {
            message = "Invalid employee credentials.";
            return false;
        }

        if (employee.Role != Employee.RoleEnum.FinanceManager)
        {
            message = $"You do not have the authorization to approve tickets.";
            return false;
        }

        Ticket? ticket = repo.GetTicketById(ticketId);
        if (ticket == null)
        {
            message = $"No ticket with that id exists.";
            return false;
        }

        if (ticket.Status != Ticket.StatusEnum.Pending)
        {
            message = "This ticket has already been resolved.";
            return false;
        }

        if (repo.ApproveTicket(ticketId, employeeId))
        {
            message = "The ticket has been successfully approved.";
            return true;
        }
        else
        {
            message = "The ticket was unable to be approved.";
            return false;
        }
    }

    public bool DenyTicket(int employeeId, int ticketId) => DenyTicket(employeeId, ticketId, out _);
    public bool DenyTicket(int employeeId, int ticketId, out string message)
    {
        // Check employee existence before creating ticket
        Employee? employee = authService.GetEmployeeById(employeeId);
        if (employee is null)
        {
            message = "Invalid employee credentials.";
            return false;
        }

        if (employee.Role != Employee.RoleEnum.FinanceManager)
        {
            message = $"You do not have the authorization to deny tickets.";
            return false;
        }

        Ticket? ticket = repo.GetTicketById(ticketId);
        if (ticket == null)
        {
            message = $"No ticket with that id exists.";
            return false;
        }

        if (ticket.Status != Ticket.StatusEnum.Pending)
        {
            message = "This ticket has already been resolved.";
            return false;
        }

        if (repo.DenyTicket(ticketId, employeeId))
        {
            message = "The ticket has been successfully denied.";
            return true;
        }
        else
        {
            message = "The ticket was unable to be denied.";
            return false;
        }
    }

    public List<Ticket> GetTicketsByEmployee(int employeeId)
    {
        return repo.GetTicketsByEmployee(employeeId);
    }

    public List<Ticket> GetAllTickets()
    {
        return repo.GetAllTickets();
    }

    public List<Ticket> GetTicketsByStatus(Ticket.StatusEnum status)
    {
        return repo.GetTicketsByStatus(status);
    }

    public Ticket? GetTicketById(int ticketId)
    {
        return repo.GetTicketById(ticketId);
    }
}
