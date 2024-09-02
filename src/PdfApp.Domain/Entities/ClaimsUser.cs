namespace PdfApp.Domain.Entities;

public class ClaimsUser
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
