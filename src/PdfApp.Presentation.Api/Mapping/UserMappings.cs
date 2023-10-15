using PdfApp.Domain.Entities;
using PdfApp.Presentation.Api.Contracts.User;

namespace PdfApp.Presentation.Api.Mapping;

public static class UserMappings
{
    public static UserResponse DomainToResponse(this User user) =>
        new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email);
}
