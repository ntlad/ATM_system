using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Services.Users;

public interface IUserService
{
    public TransactionResult GetOperationsHistory(int operationsHistoryCount);

    public TransactionResult TryDeposit(long amount);

    public TransactionResult TryWithdraw(long amount);

    public TransactionResult GetBalance();

    public TransactionResult CreateUser(string username, string password, UserRole role, long balance = 0);

    public ResultType Login(string username, string password);

    public ResultType UpdateCurrentUser(string username);
}