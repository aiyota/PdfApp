using PdfApp.Domain.Abstractions;
using System.Net;

namespace PdfApp.Domain.Exceptions;
public class InvalidFileTypeException(string fileTypeName) 
    : Exception, IServiceException
{
    private readonly string _fileTypeName = fileTypeName;

    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string ErrorMessage => $"File must be a {_fileTypeName} file";
}
