using Lab5.Application.Abstractions.Results;
using Lab5.Application.Services.Users;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.UpdateBalance;

public class UpdateBalanceScenario : IScenario
{
    private readonly IUserService _userService;

    public UpdateBalanceScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Balance Update";

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

        string operation = AnsiConsole.Ask<string>("Enter operation type");
        int amount = AnsiConsole.Ask<int>("Enter amount of money");

        TransactionResult result = operation == "Deposit"
            ? _userService.TryDeposit(amount)
            : _userService.TryWithdraw(amount);

        string message = result.Result switch
        {
            Result.Success => "Successful updated.",
            Result.Failure => result.Message,
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}