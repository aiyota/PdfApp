namespace PdfApp.Domain.Entities;

/// <summary>
/// Represents a searchable tag for a pdf (e.g. "C#", "SQL", "Python").
/// </summary>
public class Tag
{
    public int Id { get; set; }
    public int PdfId { get; set; }
    public string Name { get; set; } = default!;
}
