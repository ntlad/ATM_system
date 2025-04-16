namespace Lab5.Application.Abstractions.Results;

public enum Result
{
    Success,
    Failure,
}

public class ResultType(Result result, string message, long count)
{
    public Result Result { get; set; } = result;

    public string Message { get; set; } = message;

    public long Count { get; set; } = count;

    public ResultType(Result result) : this(result, string.Empty, 0) { }

    public ResultType(Result result, string message) : this(result, message, 0) { }
}
