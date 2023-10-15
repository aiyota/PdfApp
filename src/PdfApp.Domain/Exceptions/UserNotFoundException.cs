using PdfApp.Domain.Abstractions;
using System.Net;

namespace PdfApp.Domain.Exceptionsp;

public class UserNotFoundException : Exception, IServiceException
{
    private readonly string? _email;
    private readonly Guid? _id;

    public UserNotFoundException(string? email, Guid? id)
    {
        _email = email;
        _id = id;
    }

    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public string ErrorMessage => (_email is not null)
                                    ? $"User with email '{_email}' not found"
                                    : $"User with id '{_id}' not found";
}
