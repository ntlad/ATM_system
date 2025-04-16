using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Abstractions.Repositories;

public interface IUserRepository
{
    public User? FindUserByUsername(string username);

    public TransactionResult TryUpdateBalance(User user, decimal balance, TransactionType transactionType);

    public TransactionResult TryCreateUser(string username, string password, UserRole userRole, long balance);

    public TransactionResult TryCheckBalance(User user);
}