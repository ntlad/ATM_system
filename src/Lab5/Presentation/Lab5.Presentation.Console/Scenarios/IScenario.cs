namespace Lab5.Presentation.Console.Scenarios;

public interface IScenario
{
    public string Name { get; }

    public void Run();
}