using System.Net;

namespace PdfApp.Domain.Abstractions;

public interface IServiceException
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorMessage { get; }
}

