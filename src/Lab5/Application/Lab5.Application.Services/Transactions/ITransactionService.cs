using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Services.Transactions;

public interface ITransactionService
{
    public TransactionResult GetTransactions(long count, User user);

    public ResultType CountTransactions(string username);

    public Transaction SaveTransaction(long userId, TransactionType type);
}