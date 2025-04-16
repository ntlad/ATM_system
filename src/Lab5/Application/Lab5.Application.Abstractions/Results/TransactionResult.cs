using Lab5.Application.Models.Transactions;

namespace Lab5.Application.Abstractions.Results;

public class TransactionResult
{
    public TransactionType TransactionType { get; set; }

    public Result Result { get; set; }

    public string Message { get; set; }

    public IEnumerable<Transaction>? OperationsHistory { get; set; }

    public long Balance { get; set; }

    public TransactionResult(Result result, TransactionType transactionType)
    {
        TransactionType = transactionType;
        Result = result;
        Message = string.Empty;
    }

    public TransactionResult(Result result, TransactionType transactionType, string message)
    {
        TransactionType = transactionType;
        Result = result;
        Message = message;
    }

    public TransactionResult AddOperationsHistory(IEnumerable<Transaction> operationsHistory)
    {
        this.OperationsHistory = operationsHistory;
        return this;
    }

    public TransactionResult AddBalance(long balance)
    {
        this.Balance = balance;
        return this;
    }

    public string OperationsToString()
    {
        if (OperationsHistory != null)
        {
            string operationsString = string.Empty;
            foreach (Transaction operation in OperationsHistory)
            {
                operationsString += $"{operation.Type}\n";
            }

            return operationsString;
        }

        return "ffffffkjil";
    }
}