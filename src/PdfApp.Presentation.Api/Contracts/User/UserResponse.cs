namespace PdfApp.Presentation.Api.Contracts.User;

public record UserResponse(
    Guid Id,
    string UserName,
    string Email);
