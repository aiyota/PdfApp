﻿using PdfApp.Domain.Entities;

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
    Task<IList<Pdf>> GetAllAsync();
    Task<Pdf?> GetByIdAsync(int id);
    Task<IList<Pdf>> GetByTitleAsync(string fileName);
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
    Task<IEnumerable<Tag>> GetAllTagsAsync();
}
