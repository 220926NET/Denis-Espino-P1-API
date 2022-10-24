namespace Model;
public class Employee
{
    public Employee(int id, string username, string password, RoleEnum role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }

    public int Id {get; set;}

    public string Username {get; set;}
    public string Password {get; set;}

    public RoleEnum Role {get; set;}
    public enum RoleEnum {
        Employee,
        FinanceManager
    }

    public static string RoleString(RoleEnum role)
    {
        switch(role)
        {
            case RoleEnum.Employee:
                return "Employee";
            case RoleEnum.FinanceManager:
                return "FinanceManager";
            default:
                return "Employee role unknown";
        }

    }
}
