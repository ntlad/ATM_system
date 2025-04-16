using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;
using Lab5.Application.Services.Transactions;

namespace Lab5.Application.Transactions;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
    }

    public TransactionResult GetTransactions(long count, User user)
    {
        ResultType? resultType = _transactionRepository.CountTransactions(user);

        if (resultType == null)
        {
            return new TransactionResult(Result.Failure, TransactionType.GetHistory);
        }

        long countTransactions = resultType.Count;

        if (countTransactions < count)
        {
            count = countTransactions;
        }

        TransactionResult result = new TransactionResult(Result.Success, TransactionType.GetHistory, count.ToString())
            .AddOperationsHistory(_transactionRepository.TryGetTransactions(user.Id, count));

        _transactionRepository.TrySaveTransaction(
            user.Id,
            TransactionType.GetHistory);

        return result;
    }

    public ResultType CountTransactions(string username)
    {
        User? user = _userRepository.FindUserByUsername(username);

        if (user is null)
        {
            return new ResultType(Result.Failure, "User not found");
        }

        ResultType? resultType = _transactionRepository.CountTransactions(user);

        if (resultType == null)
        {
            return new ResultType(Result.Failure, "Transaction not found");
        }

        return resultType;
    }

    public Transaction SaveTransaction(long userId, TransactionType type)
    {
        Transaction result = _transactionRepository.TrySaveTransaction(userId, type);
        return result;
    }
}