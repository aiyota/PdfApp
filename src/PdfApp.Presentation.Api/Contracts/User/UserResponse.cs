namespace PdfApp.Presentation.Api.Contracts.User;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
