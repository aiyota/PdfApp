using PdfApp.Domain.Entities;
using PdfApp.Presentation.Api.Contracts.Pdf;

namespace PdfApp.Presentation.Api.Mapping;

public static class ProgressMappings
{
    public static ProgressResponse DomainToResponse(this Progress progress) => new()
    {
        Id = progress.Id,
        Name = progress.Name,
        Page = progress.Page
    };

    public static IEnumerable<ProgressResponse> DomainToResponse(this IEnumerable<Progress> progresses) =>
        progresses.Select(p => p.DomainToResponse());
}
