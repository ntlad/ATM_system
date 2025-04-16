using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;
using Lab5.Application.Transactions;
using Lab5.Application.Users;
using Moq;
using Xunit;
using static Lab5.Application.Abstractions.Results.Result;

namespace Lab5.Tests;

public class Test3Scenario()
{
    [Fact]
    public void Deposit_Maintaining_ReturnSuccess()
    {
        var user = new User(1, "jane", UserRole.Customer, "owowo", 350);
        var currentUser = new CurrentUserManager
        {
            User = user,
        };

        var userRepository = new Mock<IUserRepository>();
        var transactionRepository = new Mock<ITransactionRepository>();

        var transactionService = new TransactionService(
            transactionRepository.Object,
            userRepository.Object);

        var userService = new UserService(
            userRepository.Object,
            currentUser,
            transactionService);

        userRepository.Setup(repo => repo.TryUpdateBalance(user, 370, TransactionType.Deposit))
            .Returns(new TransactionResult(Result.Success, TransactionType.Deposit));

        TransactionResult result = userService.TryDeposit(20);

        Assert.Equal(Success, result.Result);
        Assert.Equal(370, user.Balance);

        transactionRepository.Setup(repo => repo.TrySaveTransaction(1, TransactionType.Withdraw))
            .Returns(new Transaction(1, 1, TransactionType.Withdraw, TransactionStatus.Completed));

        transactionRepository.Verify(
            repo => repo.TrySaveTransaction(user.Id, TransactionType.Deposit),
            Times.Once());

        userRepository.Verify(
            repo => repo.TryUpdateBalance(user, 370, TransactionType.Deposit),
            Times.Once());
    }
}