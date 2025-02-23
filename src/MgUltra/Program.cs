using DSharpPlus;
using dotenv.net;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Extensions;
using DSharpPlus.Interactivity.Extensions;
using MgUltra.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

DotEnv.Fluent().WithProbeForEnv(6).Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var discordToken = "DISCORD__TOKEN".FromEnvRequired();
var debugGuildId = ulong.Parse("DISCORD__TEST_GUILD_ID".FromEnvRequired());

var services = new ServiceCollection();

services.AddScoped<GreeterService>();

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

var status = new DiscordActivity("Machine girl", DiscordActivityType.ListeningTo);
await discordClient.ConnectAsync(status, DiscordUserStatus.Online);
await Task.Delay(-1);