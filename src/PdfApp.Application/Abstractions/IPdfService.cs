using Microsoft.AspNetCore.Http;
using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IPdfService
{
    Task<Pdf> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags);
    Task<Pdf> GetByIdAsync(int id);
    Task<IList<Pdf>> GetAllAsync();
    Task<IList<Pdf>> GetByTitleAsync(string title);
    Task<Pdf> UpdateAsync(
        int id,
        string? title = null,
        string? description = null,
        string? author = null,
        int? totalPages = null,
        string? fileName = null,
        IEnumerable<Tag>? tags = null,
        bool? hasFile = null);
    Task DeleteAsync(int id);
    Task UploadAsync(int id, IFormFile file);
    Task<byte[]> GetPdfFileAsync(string fileName);
}
