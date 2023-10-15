using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PdfApp.Presentation.Api.Common;

public class PdfAppValidationProblemDetails : ValidationProblemDetails
{
    private readonly ModelStateDictionary _modelStateDictionary;

    public PdfAppValidationProblemDetails(ModelStateDictionary modelStateDictionary)
    {
        _modelStateDictionary = modelStateDictionary;
    }

    public new IEnumerable<string> Errors
    {
        get => _modelStateDictionary.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage);
    }
}
