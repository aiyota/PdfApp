namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record PdfTagRequest
{
    public string Name { get; set; } = default!;
}
