using System.ComponentModel.DataAnnotations;

namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record UploadPdfRequest
{
    [Required]
    public IFormFile File { get; set; } = default!;
}
