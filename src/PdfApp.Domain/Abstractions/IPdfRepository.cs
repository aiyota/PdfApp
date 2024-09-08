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
        IEnumerable<Tag> tags,
        bool? hasFile);
    Task<IList<Pdf>> GetAllAsync(IList<string>? tags = null);
    Task<Pdf?> GetByIdAsync(int id);
    Task<IList<Pdf>> GetByTitleAsync(string fileName, IList<string>? tags = null);
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
    Task SaveProgressAsync(Guid userId, int pdfId, int currentPage);
    Task<IEnumerable<Progress>> GetProgressesAsync(Guid userId, int pdfId);
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task AddToFavorites(Guid userId, int pdfId);
    Task<IEnumerable<Pdf>> GetUserFavoritePdfs(Guid userId, string? title = null, IEnumerable<string>? tags = null);
    Task RemoveFromFavorites(Guid userId, int pdfId);
}
