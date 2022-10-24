using Model;
using TicketSystem;
using Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    public class RequestTicket
    {
        public int? employeeId {get; set;}
        public decimal? amount {get; set;}
        public string? description {get; set;}
    }
    private readonly ITicketService ticketService;
    private readonly IAuthService authService;
    public TicketController(ITicketService ticketService, IAuthService authService)
    {
        this.ticketService = ticketService;
        this.authService = authService;
    }

    [HttpPost("submit")]
    public ActionResult SubmitTicket(RequestTicket requestTicket)
    {
        if (requestTicket.employeeId is null)
        {
            return BadRequest("Missing employee id.");
        }
        else if (requestTicket.amount is null)
        {
            return BadRequest("Missing amount.");
        } 
        else if (requestTicket.description is null)
        {
            return BadRequest("Missing description.");
        }
        
        string message;
        if (ticketService.CreateTicket((int)requestTicket.employeeId, (decimal)requestTicket.amount, requestTicket.description, out message))
        {
            return Ok(message);
        }
        else
        {
            return BadRequest(message);
        }
    }

    [HttpGet()]
    public ActionResult TicketsByStatus(string status)
    {
        Ticket.StatusEnum? statusEnum = StatusFromString(status);
        if (statusEnum is null)
            return BadRequest("Invalid ticket status.");
        else
            return Ok(ticketService.GetTicketsByStatus((Ticket.StatusEnum)statusEnum));
    }

    [HttpGet("employee/{employeeId:int}")]
    public ActionResult TicketsByEmployee(int employeeId)
    {
        if (authService.GetEmployeeById(employeeId) is null)
            return BadRequest("Invalid employee id.");
        else
            return Ok(ticketService.GetTicketsByEmployee(employeeId));
    }

    
    [HttpPut("{ticketId}")]
    public ActionResult ProcessTicket([FromBody]string status, int ticketId)
    {
        string? employeeIdHeader = GetHeader("employeeid");
        if (employeeIdHeader is null)
            return BadRequest("Missing employeeid header.");
        
        if (!int.TryParse(employeeIdHeader, out int employeeId))
            return BadRequest("Header employeeid is not an integer.");

        switch(StatusFromString(status))
        {
            case Ticket.StatusEnum.Accepted:
            {
                if (ticketService.ApproveTicket(employeeId, ticketId, out string message))
                {
                    return Ok(message);
                }
                else
                {
                    return BadRequest(message);
                }
            }
            case Ticket.StatusEnum.Denied:
            {
                if (ticketService.DenyTicket(employeeId, ticketId, out string message))
                {
                    return Ok(message);
                }
                else
                {
                    return BadRequest(message);
                }
            }
            default:
                return BadRequest("Invalid status value.");
        }
    }
    

    private string? GetHeader(string header, int index = 0)
    {
        var strings = Request.Headers[header];
        if (index >= strings.Count)
        {
            return null;
        }
        else
        {
            return strings[index];
        }
    }

    private Ticket.StatusEnum? StatusFromString(string status)
    {
        switch (status.ToLower())
        {
            case "pending":
                return Ticket.StatusEnum.Pending;

            case "accept":
            case "accepted":
            case "approve":
            case "approved":
                return Ticket.StatusEnum.Accepted;

            case "deny":
            case "denied":
                return Ticket.StatusEnum.Denied;

            default:
                return null;
        }
    }
}