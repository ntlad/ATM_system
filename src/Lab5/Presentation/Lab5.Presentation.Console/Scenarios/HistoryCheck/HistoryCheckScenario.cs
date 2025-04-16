using Lab5.Application.Abstractions.Results;
using Lab5.Application.Services.Users;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.HistoryCheck;

public class HistoryCheckScenario : IScenario
{
    private readonly IUserService _userService;

    public HistoryCheckScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Get Operations History";

    public void Run()
    {
        string username = AnsiConsole.Ask<string>("Enter your username");

        string password = AnsiConsole.Ask<string>("Enter your pin-code");

        ResultType resultType = _userService.Login(username, password);

        string messageFirst = resultType.Result switch
        {
            Result.Success => "Successful login",
            Result.Failure => "User not found",
            _ => throw new ArgumentOutOfRangeException(nameof(resultType)),
        };

        AnsiConsole.WriteLine(messageFirst);
        AnsiConsole.Ask<string>("Ok");

        ResultType res = _userService.UpdateCurrentUser(username);
        if (res.Result == Result.Failure)
        {
            AnsiConsole.WriteLine(res.Message);
            return;
        }

        int count = AnsiConsole.Ask<int>("Enter count operations");

        TransactionResult result = _userService.GetOperationsHistory(count);
        string message = result.Result switch
        {
            Result.Success => result.OperationsToString(),
            Result.Failure => result.Message,
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}