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

    public static TagResponse DomainToResponse(this Tag tag) => new()
    {
        Id = tag.Id,
        Name = tag.Name
    };

    public static IEnumerable<TagResponse> DomainToResponse(this IEnumerable<Tag> tags) =>
        tags.Select(tag => tag.DomainToResponse());
}
