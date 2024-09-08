using PdfApp.Domain.Entities;

namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record PdfResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string? Author { get; init; }
    public int TotalPages { get; init; }
    public string FileName { get; init; } = default!;
    public IEnumerable<TagResponse> Tags { get; init; } = Enumerable.Empty<TagResponse>();
    public DateTime CreatedOn { get; init; }
    public DateTime? LastAccessedOn { get; init; }
    public bool HasFile { get; set; }
    public bool IsFavorite { get; set; }
}
