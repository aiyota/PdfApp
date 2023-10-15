using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PdfApp.Presentation.Api.Common;

/// <summary>
/// Singleton class handler of events related to JWT authentication.
/// Gets JWT from cookie and sets token.
/// </summary>
public class AuthEventsHandler : JwtBearerEvents
{
    private const string _bearerPrefix = "Bearer ";

    private AuthEventsHandler()
    {
        OnMessageReceived = MessageReceivedHandler;
    }

    /// <summary>
    /// Gets single available instance of <see cref="AuthEventsHandler"/>
    /// </summary>
    public static AuthEventsHandler Instance { get; } = new AuthEventsHandler();

    private Task MessageReceivedHandler(MessageReceivedContext context)
    {
        if (context.Request.Cookies.TryGetValue(CustomHeaders.AccessToken, out string? headerValue))
        {
            string? token = headerValue;
            if (!string.IsNullOrEmpty(token) && token.StartsWith(_bearerPrefix))
            {
                token = token[_bearerPrefix.Length..];
            }

            context.Token = token;
        }

        return Task.CompletedTask;
    }
}
