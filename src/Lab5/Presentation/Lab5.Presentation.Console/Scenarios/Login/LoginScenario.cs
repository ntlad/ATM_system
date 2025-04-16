using Lab5.Application.Abstractions.Results;
using Lab5.Application.Services.Users;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.Login;

public class LoginScenario : IScenario
{
    private readonly IUserService _userService;

    public LoginScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Login User";

    public void Run()
    {
        string username = AnsiConsole.Ask<string>("Enter your username");

        string password = AnsiConsole.Ask<string>("Enter system password");

        ResultType result = _userService.Login(username, password);

        string message = result.Result switch
        {
            Result.Success => "Successful login",
            Result.Failure => "Failed login attempt",
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}