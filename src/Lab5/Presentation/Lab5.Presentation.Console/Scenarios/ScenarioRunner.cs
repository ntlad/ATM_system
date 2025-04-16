using Lab5.Application.Models.Users;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios;

public class ScenarioRunner
{
    private readonly IEnumerable<IScenarioProvider> _providers;

    public ScenarioRunner(IEnumerable<IScenarioProvider> providers)
    {
        _providers = providers;
    }

    public void Run()
    {
        UserRole userType = AnsiConsole.Prompt(
            new SelectionPrompt<UserRole>()
                .Title("Select user type")
                .AddChoices(Enum.GetValues<UserRole>()));

        if (userType == UserRole.Admin)
        {
            IEnumerable<IScenario> scenarios = GetScenarios(0, 1);

            SelectionPrompt<IScenario> selector = new SelectionPrompt<IScenario>()
                .Title("Chose action")
                .AddChoices(scenarios)
                .UseConverter(x => x.Name);

            IScenario scenario = AnsiConsole.Prompt(selector);
            scenario.Run();
        }
        else if (userType == UserRole.Customer)
        {
            IEnumerable<IScenario> scenarios = GetScenarios(1, 5);

            SelectionPrompt<IScenario> selector = new SelectionPrompt<IScenario>()
                .Title("Select action")
                .AddChoices(scenarios)
                .UseConverter(x => x.Name);

            IScenario scenario = AnsiConsole.Prompt(selector);
            scenario.Run();
        }
    }

    private IEnumerable<IScenario> GetScenarios(int start, int end)
    {
        int current = 0;
        foreach (IScenarioProvider provider in _providers)
        {
            if (current >= start
                && current < end
                && provider is IScenarioProvider scenarioProvider
                && scenarioProvider.TryGetScenario(out IScenario? scenario))
            {
                yield return scenario;
            }

            ++current;
        }
    }
}