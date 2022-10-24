using Model;
public interface IAuthService
{
    public bool RegisterEmployee(string username, string password, out string message);
    public Employee? LoginEmployee(string username, string password, out string message);
    public Employee? GetEmployeeById(int employeeId);
}