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

public class Test2Scenario()
{
    [Fact]
    public void Withdraw_MaintainingWithInsufficientBalance_ReturnSuccess()
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

        userRepository.Setup(repo => repo.TryUpdateBalance(user, -10, TransactionType.Withdraw))
            .Returns(new TransactionResult(Result.Failure, TransactionType.Withdraw));

        TransactionResult result = userService.TryWithdraw(360);

        Assert.Equal(Failure, result.Result);

        userRepository.Verify(
            repo => repo.TryUpdateBalance(user, -10, TransactionType.Withdraw),
            Times.Never());
    }
}