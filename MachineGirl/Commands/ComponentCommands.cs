using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace MachineGirl.Commands;

public sealed class ComponentCommands
{
    [Command("button_test")]
    public static async ValueTask Button(SlashCommandContext ctx)
    {
        var message = new DiscordMessageBuilder()
            .AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Primary, "primary", "Blurple!"),
                new DiscordButtonComponent(DiscordButtonStyle.Secondary, "secondary", "Gray!"),
                new DiscordButtonComponent(DiscordButtonStyle.Success, "success", "Green!"),
                new DiscordButtonComponent(DiscordButtonStyle.Danger, "danger", "Red!"),
                new DiscordLinkButtonComponent("https://github.com/ddjerqq", "Link!"));

        await ctx.RespondAsync(message);
        var responseMsg = await ctx.GetResponseAsync();
        var btnResponse = await responseMsg!.WaitForButtonAsync();

        if (btnResponse.TimedOut)
        {
            await responseMsg!.DeleteAsync();
            return;
        }

        await btnResponse.Result.Interaction.CreateResponseAsync(
            DiscordInteractionResponseType.UpdateMessage,
            new DiscordInteractionResponseBuilder().WithContent($"You chose {btnResponse.Result.Id}"));
    }

    [Command("dropdown")]
    public static async ValueTask Dropdown(SlashCommandContext ctx)
    {
        // Create the options for the user to pick
        var options = new List<DiscordSelectComponentOption>
        {
            new("apple", "apple", string.Empty, false, new DiscordComponentEmoji("🍎")),
            new("orange", "orange", string.Empty, false, new DiscordComponentEmoji("🍊")),
            new("banana", "banana", string.Empty, false, new DiscordComponentEmoji("🍌")),
            new("grape", "grape", string.Empty, false, new DiscordComponentEmoji("🍇")),
            new("strawberry", "strawberry", string.Empty, false, new DiscordComponentEmoji("🍓")),
            new("mango", "mango", string.Empty, false, new DiscordComponentEmoji("🥭")),
        };

        var dropdown = new DiscordSelectComponent("dropdown", "apple", options);
        await ctx.RespondAsync(new DiscordMessageBuilder().AddComponents(dropdown));

        var responseMsg = await ctx.GetResponseAsync();
        var interResponse = await responseMsg!.WaitForSelectAsync(_ => true);

        if (interResponse.TimedOut)
        {
            await responseMsg!.DeleteAsync();
            return;
        }

        await interResponse.Result.Interaction.CreateResponseAsync(
            DiscordInteractionResponseType.UpdateMessage,
            new DiscordInteractionResponseBuilder().WithContent($"You chose {interResponse.Result.Values[0]}"));
    }
}