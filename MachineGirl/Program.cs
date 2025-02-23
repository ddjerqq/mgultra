using DSharpPlus;
using dotenv.net;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Entities;
using DSharpPlus.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using MachineGirl.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

DotEnv.Fluent().WithProbeForEnv(6).Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var discordToken = "DISCORD__TOKEN".FromEnvRequired();
var debugGuildId = ulong.Parse("DISCORD__TEST_GUILD_ID".FromEnvRequired());

var services = new ServiceCollection();

services.AddSerilog();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, true));

services.AddInteractivityExtension();
services.AddDiscordClient(discordToken, SlashCommandProcessor.RequiredIntents);
services.AddCommandsExtension((sp, ext) =>
{
    ext.AddCommands(typeof(Program).Assembly, debugGuildId);
});

var sp = services.BuildServiceProvider();
var discordClient = sp.GetRequiredService<DiscordClient>();

var status = new DiscordActivity("Dance in the fire", DiscordActivityType.Custom);
await discordClient.ConnectAsync(status, DiscordUserStatus.Online);
await Task.Delay(-1);