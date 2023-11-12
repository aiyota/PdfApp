using PdfApp.Domain.Abstractions;
using System.Net;

namespace PdfApp.Domain.Exceptions;

public class PdfNotFoundException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public string ErrorMessage => "The requested PDF was not found";
}
