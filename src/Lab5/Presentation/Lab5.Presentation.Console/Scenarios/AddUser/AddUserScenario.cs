using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Users;
using Lab5.Application.Services.Users;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.AddUser;

public class AddUserScenario : IScenario
{
    private readonly IUserService _userService;

    public AddUserScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Add new user";

    public void Run()
    {
        string username = AnsiConsole.Ask<string>("Enter your username");
        string role = AnsiConsole.Ask<string>("Check your role");
        string password = AnsiConsole.Ask<string>("Enter your password");

        UserRole user_role = role == "Admin" ? UserRole.Admin : UserRole.Customer;

        TransactionResult result = _userService.CreateUser(
            username,
            password,
            user_role);

        string message = result.Result switch
        {
            Result.Success => "Successful creating user",
            Result.Failure => result.Message,
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}