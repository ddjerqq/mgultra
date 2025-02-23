using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;

namespace MgUltra.Commands;

public sealed class AutoCompleteCommands
{
    [Command("ping")]
    public static async ValueTask ExecuteAsync(SlashCommandContext context) =>
        await context.RespondAsync("Pong!");

    [Command("choices")]
    public static async ValueTask ExecuteAsync(SlashCommandContext context, [SlashChoiceProvider<DaysOfTheWeekProvider>] int day) =>
        await context.RespondAsync($"You chose: {day}");

    [Command("autocomplete")]
    public static async ValueTask ExecuteAsync(SlashCommandContext context, [SlashAutoCompleteProvider<TagNameAutoCompleteProvider>] string tagName) =>
        await context.RespondAsync($"You chose: {tagName}");
}

public class DaysOfTheWeekProvider : IChoiceProvider
{
    private static readonly IReadOnlyList<DiscordApplicationCommandOptionChoice> DaysOfTheWeek =
    [
        new("Sunday", 0),
        new("Monday", 1),
        new("Tuesday", 2),
        new("Wednesday", 3),
        new("Thursday", 4),
        new("Friday", 5),
        new("Saturday", 6),
    ];

    public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) =>
        ValueTask.FromResult(DaysOfTheWeek.AsEnumerable());
}

public class TagNameAutoCompleteProvider : IAutoCompleteProvider
{
    private static readonly IReadOnlyList<string> Tags =
    [
        "foo",
        "bar",
        "baz",
    ];

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext context)
    {
        var tags = Tags
            .Select((tag, idx) => new { Name = tag, Id = idx })
            .Where(x => x.Name.Contains(context.UserInput ?? string.Empty))
            .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Id.ToString()));

        return ValueTask.FromResult(tags);
    }
}