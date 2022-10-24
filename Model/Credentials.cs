namespace Model;
public class Credentials
{
    public Credentials(int id, string username, Employee.RoleEnum role)
    {
        Id = id;
        Username = username;
        Role = role;
    }

    public int Id {get; set;}

    public string Username {get; set;}

    public Employee.RoleEnum Role {get; set;}
}