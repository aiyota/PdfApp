﻿namespace PdfApp.Domain.Entities;

public class Pdf
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? Author { get; set; }
    public int TotalPages { get; set; }
    public string FileName { get; set; } = default!;
    public IEnumerable<Tag> Tags { get; set; } = default!;  
    public DateTime CreatedOn { get; set; }
    public DateTime? LastAccessedOn { get; set; } 
    public bool? HasFile { get; set; }
    public ICollection<FavoritePdf> FavoritedBy { get; set; } = default!;
}
