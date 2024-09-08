using Microsoft.AspNetCore.Http;
using PdfApp.Application.Models;
using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IPdfService
{
    Task<UserPdf> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags,
        bool? hasFile);
    Task<UserPdf> GetByIdAsync(Guid userId, int id);
    Task<IList<UserPdf>> GetAllAsync(Guid userId, IList<string>? tags = null);
    Task<IList<UserPdf>> GetByTitleAsync(Guid userId, string title, IList<string>? tags = null);
    Task<UserPdf> UpdateAsync(
        Guid userId,
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
    Task<IList<Tag>> GetTagsAsync();
    Task SaveProgressAsync(Guid userId, int pdfId, int currentPage);
    Task<IList<Progress>> GetProgressesAsync(Guid userId, int pdfId);
    Task AddToFavorites(Guid userId, int pdfId);
    Task<IList<UserPdf>> GetUserFavoritePdfs(Guid userId, string? title = null, IEnumerable<string>? tags = null);
    Task RemoveFromFavorites(Guid userId, int pdfId);
}
