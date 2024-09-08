namespace PdfApp.Domain.Entities;

public class FavoritePdf
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int PdfId { get; set; }

    public User User { get; set; } = default!;
    public Pdf Pdf { get; set; } = default!;
}