using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PdfApp.Application.Abstractions;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Exceptions;

namespace PdfApp.Application.Services;

public class PdfService : IPdfService
{
    private readonly IPdfRepository _pdfRepository;
    private readonly IConfiguration _configuration;

    public PdfService(IPdfRepository pdfRepository, IConfiguration configuration)
    {
        _pdfRepository = pdfRepository;
        _configuration = configuration;
    }

    public async Task<Pdf> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags,
        bool? hasFile)
    {
        var created = await _pdfRepository.CreateAsync(
            title,
            description,
            author,
            totalPages,
            fileName,
            tags, 
            hasFile) 
                ?? throw new PdfNotFoundException();

        return created;
    }

    public async Task DeleteAsync(int id)
    {
        var pdf = await _pdfRepository.GetByIdAsync(id);
        if (pdf is null)
            throw new PdfNotFoundException();

        await _pdfRepository.DeleteAsync(id);

        DeletePdfFile(pdf.FileName);
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
        IEnumerable<Tag>? tags,
        bool? hasFile)
    {
        try
        {
            var updated = await _pdfRepository.UpdateAsync(
                id,
                title,
                description,
                author,
                totalPages,
                fileName,
                tags,
                hasFile)
                    ?? throw new PdfNotFoundException();

            return updated;
        }
        catch (InvalidOperationException)
        {
            throw new PdfNotFoundException();
        }

    }

    public async Task UploadAsync(int id, IFormFile file)
    {
        var pdfRecord = await _pdfRepository.GetByIdAsync(id) 
            ?? throw new PdfNotFoundException();

        string pdfFilePath = _configuration.GetValue<string>("PdfFilePath")!;
        string filePath = Path.Combine(pdfFilePath, pdfRecord.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        await _pdfRepository.UpdateAsync(id, hasFile: true);
    }

    public async Task<byte[]> GetPdfFileAsync(string fileName)
    {
        string pdfFilePath = _configuration.GetValue<string>("PdfFilePath")!;
        string filePath = Path.Combine(pdfFilePath, fileName);
        if (!File.Exists(filePath))
            throw new PdfNotFoundException();

        return await File.ReadAllBytesAsync(filePath); 
    }

    private bool DeletePdfFile(string fileName)
    {
        string pdfFilePath = _configuration.GetValue<string>("PdfFilePath")!;
        string filePath = Path.Combine(pdfFilePath, fileName);
        if (!File.Exists(filePath))
            return false;
   
        File.Delete(filePath);
        return true;
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await _pdfRepository.GetAllTagsAsync();
    }
}
