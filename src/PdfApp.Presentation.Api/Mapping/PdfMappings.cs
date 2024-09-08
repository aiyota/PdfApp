using PdfApp.Application.Models;
using PdfApp.Domain.Entities;
using PdfApp.Presentation.Api.Contracts.Pdf;

namespace PdfApp.Presentation.Api.Mapping;

public static class PdfMappings
{
    public static PdfResponse DomainToResponse(this UserPdf pdf) =>
        new()
        {
            Id = pdf.Id,
            Title = pdf.Title,
            Description = pdf.Description,
            Author = pdf.Author,
            TotalPages = pdf.TotalPages,
            FileName = pdf.FileName,
            Tags = pdf.Tags.Select(t => new TagResponse { Id = t.Id, Name = t.Name }),
            CreatedOn = pdf.CreatedOn,
            LastAccessedOn = pdf.LastAccessedOn,
            HasFile = pdf.HasFile ?? false,
            IsFavorite = pdf.IsFavorite
        };

    public static IList<PdfResponse> DomainToResponse(this IList<UserPdf> pdfs) =>
        pdfs.Select(pdf => pdf.DomainToResponse()).ToList();

}
