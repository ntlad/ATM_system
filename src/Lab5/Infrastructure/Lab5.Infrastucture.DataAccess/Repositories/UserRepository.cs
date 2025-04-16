using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;
using Npgsql;

namespace Lab5.Infrastucture.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public UserRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public User? FindUserByUsername(string username)
    {
        const string sql = """
         select user_id, user_name, user_role, password, balance
         from users
         where user_name = :username;
         """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", username);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() is false)
            return null;

        return new User(
            reader.GetInt64(0),
            reader.GetString(1),
            reader.GetFieldValue<UserRole>(2),
            reader.GetString(3),
            reader.GetInt64(4));
    }

    public TransactionResult TryUpdateBalance(User user, decimal balance, TransactionType transactionType)
    {
        const string sql = """
       update users
       set balance = :balance
       where user_id = :user_id;
       """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("user_id", user.Id)
            .AddParameter("balance", balance);

        command.ExecuteNonQuery();

        return new TransactionResult(Result.Success, transactionType);
    }

    public TransactionResult TryCreateUser(string username, string password, UserRole userRole, long balance)
    {
        const string sql = """
       insert into users (user_name, user_role, password, balance)
       values (:user_name, :user_role, :password, :balance);
       """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("user_name", username)
            .AddParameter("user_role", userRole)
            .AddParameter("password", password)
            .AddParameter("balance", balance);

        command.ExecuteNonQuery();

        return new TransactionResult(Result.Success, TransactionType.Create);
    }

    public TransactionResult TryCheckBalance(User user)
    {
        const string sql = """
       select balance
       from users
       where user_id = :user_id;
       """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("user_id", user.Id);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() is false)
            return new TransactionResult(Result.Failure, TransactionType.CheckBalance, "Can't check balance");

        return new TransactionResult(Result.Success, TransactionType.CheckBalance, "Check balance").
            AddBalance(reader.GetInt64(0));
    }
}