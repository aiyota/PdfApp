namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record ProgressResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Page { get; set; }
}
