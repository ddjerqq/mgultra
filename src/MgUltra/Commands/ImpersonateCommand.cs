using System.ComponentModel;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using Serilog;

namespace MgUltra.Commands;

public sealed class ImpersonateCommand
{
    [Command("impersonate"), Description("Impersonate someone")]
    public static async ValueTask ExecuteAsync(SlashCommandContext ctx, DiscordMember member, string content)
    {
        Log.Information("{User} is impersonating {Member} with content: {Content}", ctx.Interaction.User, member.DisplayName, content);
        await ctx.RespondAsync($"{member.Username} trolled successfully haha", true);
        var avatarUrl = member.GetGuildAvatarUrl(MediaFormat.WebP);

        var http = new HttpClient();
        var response = await http.GetAsync(avatarUrl);
        var avatarStream = await response.Content.ReadAsByteArrayAsync();
        await using var ms = new MemoryStream();
        await ms.WriteAsync(avatarStream);

        var webhook = await ctx.Channel.CreateWebhookAsync(member.DisplayName, ms, "trolling");

        await webhook.ExecuteAsync(new DiscordWebhookBuilder().WithContent(content));
        await webhook.DeleteAsync();
    }
}