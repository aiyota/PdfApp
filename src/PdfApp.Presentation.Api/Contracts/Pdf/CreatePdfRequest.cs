using PdfApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record CreatePdfRequest
{
    [Required]
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string? Author { get; init; }
    [Required]
    public int TotalPages { get; init; }
    [Required]
    public string FileName { get; init; } = default!;
    public IEnumerable<Tag> Tags { get; init; } = Enumerable.Empty<Tag>();
};