using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using IpScanner;
using MgUltra.Services;

namespace MgUltra.Commands;

public sealed class MicroserviceCommand(GreeterService greeter)
{
    [Command("greet"), Description("Calls the greeter micro-service")]
    public async ValueTask ExecuteAsync(SlashCommandContext ctx, DiscordMember member)
    {
        var response = await greeter.SayHello(new HelloRequest { Name = member.DisplayName });
        await ctx.RespondAsync(response.Message);
    }
}