using Lab5.Presentation.Console.Scenarios;
using Lab5.Presentation.Console.Scenarios.AddUser;
using Lab5.Presentation.Console.Scenarios.GetBalance;
using Lab5.Presentation.Console.Scenarios.HistoryCheck;
using Lab5.Presentation.Console.Scenarios.Login;
using Lab5.Presentation.Console.Scenarios.UpdateBalance;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Presentation.Console.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddScoped<ScenarioRunner>();
        collection.AddScoped<IScenarioProvider, LoginScenarioProvider>();
        collection.AddScoped<IScenarioProvider, UpdateBalanceScenarioProvider>();
        collection.AddScoped<IScenarioProvider, AddUserScenarioProvider>();
        collection.AddScoped<IScenarioProvider, GetBalanceScenarioProvider>();
        collection.AddScoped<IScenarioProvider, HistoryCheckScenarioProvider>();

        return collection;
    }
}