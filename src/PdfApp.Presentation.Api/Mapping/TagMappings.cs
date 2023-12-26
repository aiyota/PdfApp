using PdfApp.Domain.Entities;
using PdfApp.Presentation.Api.Contracts.Pdf;

namespace PdfApp.Presentation.Api.Mapping;

public static class TagMappings
{
    public static Tag RequestToDomain(this PdfTagRequest tagRequest) => new()
    {
        Name = tagRequest.Name
    };

    public static IEnumerable<Tag> RequestToDomain(this IEnumerable<PdfTagRequest> tagRequests) =>
        tagRequests.Select(tagRequest => tagRequest.RequestToDomain());
}
