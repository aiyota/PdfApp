namespace PdfApp.Presentation.Api.Contracts.User;

public record UserUpdateRequest(
    string? Email,
    string? FirstName,
    string? LastName,
    string? Password);
