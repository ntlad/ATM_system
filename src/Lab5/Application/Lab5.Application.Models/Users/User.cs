using Lab5.Application.Models.Transactions;

namespace Lab5.Application.Models.Users;

public class User
{
    public UserRole UserRole { get; set; }

    public long Id { get; set; }

    public string UserName { get; set; }

    public long Balance { get; set; }

    public IEnumerable<Transaction>? OperationsHistory { get; set; }

    public string Password { get; set; }

    public User(long id, string name, UserRole role, string password, long balance)
    {
        UserRole = role;
        Id = id;
        UserName = name;
        Password = password;
        Balance = balance;
    }

    public User(long id, string name, UserRole role, string password)
    {
        UserRole = role;
        Id = id;
        UserName = name;
        Password = password;
        Balance = 0;
    }
}