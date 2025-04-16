using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;
using Lab5.Application.Services.Transactions;
using Lab5.Application.Services.Users;

namespace Lab5.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ITransactionService _transactionService;
    private readonly ICurrentUserService _currentUserManager;

    public UserService(IUserRepository repository, ICurrentUserService currentUserManager,  ITransactionService transactionService)
    {
        _repository = repository;
        _currentUserManager = currentUserManager;
        _transactionService = transactionService;
    }

    public TransactionResult GetOperationsHistory(int operationsHistoryCount)
    {
        User? user = _currentUserManager.User;

        if (user is null)
        {
            return new TransactionResult(Result.Failure, TransactionType.GetHistory, "User not found");
        }

        return _transactionService.GetTransactions(operationsHistoryCount, user);
    }

    public TransactionResult TryDeposit(long amount)
    {
        User? user = _currentUserManager.User;

        if (user is null)
        {
            return new TransactionResult(Result.Failure, TransactionType.Deposit, "User not found");
        }

        TransactionResult result = _repository
            .TryUpdateBalance(user, user.Balance + amount, TransactionType.Deposit)
            .AddBalance(user.Balance + amount);

        if (result.Result == Result.Success)
        {
            UpdateBalance(user.Balance + amount);
        }

        _transactionService.SaveTransaction(
                user.Id,
                TransactionType.Deposit);

        return result;
    }

    public TransactionResult TryWithdraw(long amount)
    {
        User? user = _currentUserManager.User;

        if (user is null)
        {
            return new TransactionResult(Result.Failure, TransactionType.Withdraw, "User not found");
        }

        if (user.Balance < amount)
        {
            return new TransactionResult(Result.Failure, TransactionType.Withdraw, "Insufficient balance");
        }

        TransactionResult result = _repository
            .TryUpdateBalance(user, user.Balance - amount, TransactionType.Withdraw)
            .AddBalance(user.Balance - amount);

        if (result.Result == Result.Success)
        {
            UpdateBalance(user.Balance - amount);
        }

        _transactionService.SaveTransaction(
            user.Id,
            TransactionType.Withdraw);

        return result;
    }

    public TransactionResult GetBalance()
    {
        User? user = _currentUserManager.User;

        if (user is null)
        {
            return new TransactionResult(Result.Failure, TransactionType.CheckBalance, "User not found");
        }

        TransactionResult result = _repository.TryCheckBalance(user).AddBalance(user.Balance);

        _transactionService.SaveTransaction(
            user.Id,
            TransactionType.CheckBalance);

        return result;
    }

    public TransactionResult CreateUser(string username, string password, UserRole role, long balance = 0)
    {
        if (_repository.FindUserByUsername(username) is not null)
        {
            return new TransactionResult(Result.Failure, TransactionType.Create, "User already exists");
        }

        return _repository.TryCreateUser(username, password, role, balance);
    }

    public ResultType Login(string username, string password)
    {
        User? user = _repository.FindUserByUsername(username);

        if (user is null)
        {
            return new ResultType(Result.Failure, "Username or password is incorrect");
        }

        _currentUserManager.User = user;
        return new ResultType(Result.Success);
    }

    public ResultType UpdateCurrentUser(string username)
    {
        User? user = _repository.FindUserByUsername(username);

        if (user is null)
        {
            return new ResultType(Result.Failure, "User not found");
        }

        _currentUserManager.User = user;
        return new ResultType(Result.Success);
    }

    private void UpdateBalance(long newBalance)
    {
        if (_currentUserManager.User != null)
            _currentUserManager.User.Balance = newBalance;
    }
}