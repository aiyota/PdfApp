namespace PdfApp.Domain.Entities;

/// <summary>
/// Represents a progress of a user reading a pdf.
/// The `Page` property is the last page the user has read.
/// </summary>
public class Progress
{
    public int Id { get; set; }
    public int PdfId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public int Page { get; set; }
}
