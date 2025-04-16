using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Abstractions.Results;
using Lab5.Application.Models.Transactions;
using Lab5.Application.Models.Users;
using Npgsql;

namespace Lab5.Infrastucture.DataAccess.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public TransactionRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public ResultType? CountTransactions(User user)
    {
        const string sql = """
       select count(*)
       from transactions
       where user_id = :user_id
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
            return null;

        return new ResultType(Result.Success, "d", reader.GetInt64(ordinal: 0));
    }

    public Transaction TrySaveTransaction(long userId, TransactionType transactionType)
    {
         const string sql = """
         insert into transactions (user_id, transaction_type)
         values (:user_id, :transaction_type);
         """;

         NpgsqlConnection connection = _connectionProvider
             .GetConnectionAsync(default)
             .AsTask()
             .GetAwaiter()
             .GetResult();

         using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
             .AddParameter("user_id", userId)
             .AddParameter("transaction_type", transactionType.ToString());

         command.ExecuteNonQuery();

         IEnumerable<Transaction> list = TryGetTransactions(userId, 1);
         Transaction transaction = list.First();
         return new Transaction(transaction.Id, transaction.UserId, transaction.Type, transaction.Status);
    }

    public IEnumerable<Transaction> TryGetTransactions(long userId, long count)
    {
        const string sql = """
       select transaction_id, user_id, transaction_type
       from transactions
       where user_id = :user_id
       order by transaction_id desc
       limit :count;
       """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("user_id", userId)
            .AddParameter("count", count);

        using NpgsqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new Transaction(
                reader.GetInt64(0),
                reader.GetInt64(1),
                reader.GetFieldValue<string>(2),
                TransactionStatus.Completed);
        }
    }
}