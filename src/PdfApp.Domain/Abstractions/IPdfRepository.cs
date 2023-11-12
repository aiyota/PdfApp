using PdfApp.Domain.Entities;

namespace PdfApp.Domain.Abstractions;

public interface IPdfRepository
{
    Task<Pdf?> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags);
    Task<IList<Pdf>> GetAllAsync();
    Task<Pdf?> GetByIdAsync(int id);
    Task<IList<Pdf>> GetByTitleAsync(string fileName);
    Task<Pdf> UpdateAsync(
        int id,
        string? title,
        string? description,
        string? author,
        int? totalPages,
        string? fileName,
        IEnumerable<Tag>? tags);
    Task DeleteAsync(int id);
}
