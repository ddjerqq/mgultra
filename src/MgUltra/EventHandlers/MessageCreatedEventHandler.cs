using System.Globalization;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace MgUltra.EventHandlers;

public static partial class MessageCreatedEventHandler
{
    [GeneratedRegex(@"\b(?:(1[0-2]|0?[1-9])(:[0-5][0-9])?\s?(am|pm)|([01]?[0-9]|2[0-3]):[0-5][0-9])\b", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex TimestampRegex();
    
    public static async Task HandleEventAsync(DiscordClient sender, MessageCreatedEventArgs eventArgs)
    {
        var message = eventArgs.Message.Content;
        var matches = TimestampRegex().Matches(message);

        foreach (Match match in matches)
        {
            var timeString = match.Value;
            
            var formats = new[] { "h:mmtt", "htt", "HH:mm", "H:mm", "hh tt", "h tt" };
            if (DateTime.TryParseExact(timeString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedTime))
            {
                // Assume time is for today in UTC
                var now = DateTime.UtcNow;
                var dateTime = new DateTime(now.Year, now.Month, now.Day, parsedTime.Hour, parsedTime.Minute, 0, DateTimeKind.Utc);
                var unix = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
                await eventArgs.Message.RespondAsync($"<t:{unix}:t>\n-# Assuming you sent an UTC time");
            }
        }
    }
}