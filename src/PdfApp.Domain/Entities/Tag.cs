namespace PdfApp.Domain.Entities;

/// <summary>
/// Represents a searchable tag for a pdf (e.g. "C#", "SQL", "Python").
/// </summary>
public class Tag
{
    public int Id { get; set; }
    public IEnumerable<Pdf> Pdf { get; set; } = default!;   
    public string Name { get; set; } = default!;
}
