using Model;
using Serilog;
namespace Auth;
public class AuthService : IAuthService
{
    private AuthRepo authRepo;
    public AuthService()
    {
        authRepo = new AuthRepo();
    }

    public bool RegisterEmployee(string username, string password) => RegisterEmployee(username, password, out _);

    public bool RegisterEmployee(string username, string password, out string message)
    {
        if (!isUsernameValid(username, out message) || !IsPasswordValid(password, out message))
        {
            return false;
        }

        message = "Employee registered.";
        return (authRepo.CreateEmployee(username, password, Employee.RoleEnum.Employee));
    }

    public Employee? LoginEmployee(string username, string password) => LoginEmployee(username, password, out _);

    public Employee? LoginEmployee(string username, string password, out string message)
    {
        Employee? employee = authRepo.GetEmployeeByUsername(username);
        if (employee == null || employee.Password != password)
        {
            message = "Username or password is incorrect.";
            return null;
        }
        else
        {
            message = "Log in successful.";
            return employee;
        }
    }

    public bool IsPasswordValid(string password) => IsPasswordValid(password, out _);

    public bool IsPasswordValid(string password, out string message)
    {
        if (String.IsNullOrWhiteSpace(password))
        {
            message = "Password cannot be empty";
            return false;
        }

        if (password.Length < 4)
        {
            message = "Password must be 4 characters or longer.";
            return false;
        }

        message = "Password is acceptable.";
        return true;
    }

    public bool isUsernameValid(string username) => isUsernameValid(username, out _);

    public bool isUsernameValid(string username, out string message)
    {
        if (String.IsNullOrWhiteSpace(username))
        {
            message = "Username is empty.";
            return false;
        }

        if (username.Length < 4)
        {
            message = "Username must be 4 characters or longer.";
            return false;
        }

        if (!Char.IsAscii(username[0]))
        {
            message = "Username must begin with a letter of the alphabet.";
            return false;
        }

        if (authRepo.GetEmployeeByUsername(username) != null)
        {
            message = "That username is already in use.";
            return false;
        }

        message = "Username is acceptable.";
        return true;
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        return authRepo.GetEmployeeById(employeeId);
    }
}
