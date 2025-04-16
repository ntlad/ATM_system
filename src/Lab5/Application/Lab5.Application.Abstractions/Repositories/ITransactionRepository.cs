using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    public ResultType? CountTransactions(User user);

    public IEnumerable<Transaction> TryGetTransactions(long userId, long count);

    public Transaction TrySaveTransaction(long userId, TransactionType transactionType);
}