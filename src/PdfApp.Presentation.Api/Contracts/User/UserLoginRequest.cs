namespace PdfApp.Presentation.Api.Contracts.User;

public record UserLoginRequest(
    string Email,
    string Password);
