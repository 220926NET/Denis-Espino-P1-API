using Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public class RequestUser
    {
        public string? username {get; set;}
        public string? password {get; set;}
    }

    private readonly IAuthService authService;
    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    public ActionResult AuthRegister([FromBody] RequestUser requestUser)
    {
        if (requestUser.username is null)
        {
            return BadRequest("Missing username.");
        }
        else if (requestUser.password is null)
        {
            return BadRequest("Missing password.");
        }

        string message;
        if (authService.RegisterEmployee(requestUser.username, requestUser.password, out message))
        {
            return Ok(message);
        }
        else
        {
            return BadRequest(message);
        }
    }

    [HttpPost("login")]
    public ActionResult AuthLogin([FromBody] RequestUser requestUser)
    {
        if (requestUser.username is null)
        {
            return BadRequest("Missing username.");
        }
        else if (requestUser.password is null)
        {
            return BadRequest("Missing password.");
        }

        string message;
        Employee? employee = authService.LoginEmployee(requestUser.username, requestUser.password, out message);
        if (employee is null)
        {
            return BadRequest(message);
        }
        else
        {
            HttpContext.Response.Headers.Add("Employee-Id", employee.Id.ToString());
            HttpContext.Response.Headers.Add("Employee-Username", employee.Username);
            HttpContext.Response.Headers.Add("Employee-Role", Employee.RoleString(employee.Role));
            return Ok(message);
        }
    }
}