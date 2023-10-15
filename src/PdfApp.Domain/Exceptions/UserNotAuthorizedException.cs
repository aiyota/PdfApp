using PdfApp.Domain.Abstractions;
using System.Net;

namespace PdfApp.Domain.Exceptions;

public class UserNotAuthorizedException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public string ErrorMessage => "User is not authorized to make this change";
}
