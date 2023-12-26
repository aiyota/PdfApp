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
    public IEnumerable<PdfTagRequest> Tags { get; init; } = Enumerable.Empty<PdfTagRequest>();
};