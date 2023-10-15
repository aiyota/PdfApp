using System.ComponentModel.DataAnnotations;

namespace PdfApp.Presentation.Api.Contracts.User;

public record RegisterRequest(
    [Required]
    string UserName,

    [Required]
    string FirstName,

    [Required]
    string LastName,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(8)]
    string Password
);
