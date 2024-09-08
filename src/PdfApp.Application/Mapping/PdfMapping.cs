using PdfApp.Application.Models;
using PdfApp.Domain.Entities;

namespace PdfApp.Application.Mapping;

internal static class PdfMapping
{
    public static Pdf ToDomain(this UserPdf userPdf)
    {
        return new Pdf
        {
            Id = userPdf.Id,
            Title = userPdf.Title,
            Description = userPdf.Description,
            Author = userPdf.Author,
            TotalPages = userPdf.TotalPages,
            FileName = userPdf.FileName,
            Tags = userPdf.Tags,
            CreatedOn = userPdf.CreatedOn,
            LastAccessedOn = userPdf.LastAccessedOn,
            HasFile = userPdf.HasFile
        };
    }

    public static IList<Pdf> ToDomain(this IEnumerable<UserPdf> userPdfs)
    {
        return userPdfs.Select(ToDomain).ToList();
    }

    public static UserPdf ToModel(this Pdf pdf)
    {
        return new UserPdf
        {
            Id = pdf.Id,
            Title = pdf.Title,
            Description = pdf.Description,
            Author = pdf.Author,
            TotalPages = pdf.TotalPages,
            FileName = pdf.FileName,
            Tags = pdf.Tags,
            CreatedOn = pdf.CreatedOn,
            LastAccessedOn = pdf.LastAccessedOn,
            HasFile = pdf.HasFile
        };
    }

    public static IList<UserPdf> ToModel(this IEnumerable<Pdf> pdfs)
    {
        return pdfs.Select(ToModel).ToList();
    }
}
