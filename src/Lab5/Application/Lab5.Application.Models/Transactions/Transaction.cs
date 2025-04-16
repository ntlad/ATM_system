namespace Lab5.Application.Models.Transactions;

public class Transaction
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public TransactionType Type { get; set; }

    public TransactionStatus Status { get; set; }

    public Transaction(long id, long userId, TransactionType type, TransactionStatus status)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Status = status;
    }

    public Transaction(long id, long userId, string type, TransactionStatus status)
    {
        Id = id;
        UserId = userId;
        Type = type switch
        {
            "GetHistory" => TransactionType.GetHistory,
            "CheckBalance" => TransactionType.CheckBalance,
            "Deposit" => TransactionType.Deposit,
            "Withdraw" => TransactionType.Withdraw,
            "Create" => TransactionType.Create,
            _ => throw new Exception($"Unknown transaction type: {type}"),
        };
        Status = status;
    }
}