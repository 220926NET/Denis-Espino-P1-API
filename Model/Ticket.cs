namespace Model;

public class Ticket
{
    public Ticket(int id, int openedById, int closedById, decimal amount, string description, StatusEnum status, string? openedByUsername = null, string? closedByUsername = null)
    {
        Id = id;
        OpenedById = openedById;
        ClosedById = closedById;
        Amount = amount;
        Description = description;
        Status = status;
        OpenedByUsername = openedByUsername;
        ClosedByUsername = closedByUsername;
    }

    public int Id {get; set;}
    public int OpenedById {get; set;}
    public int ClosedById {get; set;}
    public decimal Amount {get; set;}
    public string Description {get; set;}
    public StatusEnum Status {get; set;}
    public string? OpenedByUsername {get; set;}
    public string? ClosedByUsername {get; set;}

    public enum StatusEnum {
        Pending,
        Accepted,
        Denied
    }

    public static string StatusString(StatusEnum status)
    {
        switch(status)
        {
            case StatusEnum.Pending:
                return "Pending";
            case StatusEnum.Accepted:
                return "Accepted";
            case StatusEnum.Denied:
                return "Denied";
            default:
                return "Ticket Status Unknown";
        }
    }
}