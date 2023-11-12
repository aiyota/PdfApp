using PdfApp.Domain.Entities;

namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record UpdatePdfRequest
{
    public string? Title { get; init; } = default!;
    public string? Description { get; init; }
    public string? Author { get; init; }
    public int? TotalPages { get; init; }
    public string? FileName { get; init; } = default!;
    public IEnumerable<Tag>? Tags { get; init; }
}
