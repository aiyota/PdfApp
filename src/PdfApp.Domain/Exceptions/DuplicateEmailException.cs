using PdfApp.Domain.Abstractions;
using System.Net;

namespace PdfApp.Domain.Exceptions;

public class DuplicateEmailException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => "Account with this email already exists.";
}
