namespace PdfApp.Presentation.Api.Contracts.Pdf;

public record TagResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
}
