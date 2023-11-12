using PdfApp.Application.Abstractions;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Exceptions;

namespace PdfApp.Application.Services;

public class PdfService : IPdfService
{
    private readonly IPdfRepository _pdfRepository;

    public PdfService(IPdfRepository pdfRepository)
    {
        _pdfRepository = pdfRepository;
    }

    public async Task<Pdf> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags)
    {
        var created = await _pdfRepository.CreateAsync(
            title,
            description,
            author,
            totalPages,
            fileName,
            tags) 
                ?? throw new PdfNotFoundException();

        return created;
    }

    public async Task DeleteAsync(int id)
    {
        var pdf = await _pdfRepository.GetByIdAsync(id);
        if (pdf is null)
            throw new PdfNotFoundException();

        await _pdfRepository.DeleteAsync(id);
    }

    public async Task<IList<Pdf>> GetAllAsync()
    {
        return await _pdfRepository.GetAllAsync();
    }

    public async Task<Pdf> GetByIdAsync(int id)
    {
        var pdf = await _pdfRepository.GetByIdAsync(id)
            ?? throw new PdfNotFoundException();
     
        return pdf;
    }

    public async Task<IList<Pdf>> GetByTitleAsync(string title)
    {
        return await _pdfRepository.GetByTitleAsync(title);
    }

    public async Task<Pdf> UpdateAsync(
        int id,
        string? title,
        string? description,
        string? author,
        int? totalPages,
        string? fileName,
        IEnumerable<Tag>? tags)
    {
        var updated = await _pdfRepository.UpdateAsync(
            id,
            title,
            description,
            author,
            totalPages,
            fileName,
            tags)
                ?? throw new PdfNotFoundException();

        return updated;
    }
}
