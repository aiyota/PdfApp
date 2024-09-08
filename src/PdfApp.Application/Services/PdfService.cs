using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Mapping;
using PdfApp.Application.Models;
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

    public async Task<UserPdf> CreateAsync(
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

        return created.ToModel();
    }

    public async Task DeleteAsync(int id)
    {
        var pdf = await _pdfRepository.GetByIdAsync(id);
        if (pdf is null)
            throw new PdfNotFoundException();

        await _pdfRepository.DeleteAsync(id);

        DeletePdfFile(pdf.FileName);
    }

    public async Task<IList<UserPdf>> GetAllAsync(Guid userId, IList<string>? tags = null)
    {
        var pdfs = await _pdfRepository.GetAllAsync(tags);
        return await SetFavorites(userId, pdfs);
    }

    public async Task<UserPdf> GetByIdAsync(Guid userId, int id)
    {
        var pdf = await _pdfRepository.GetByIdAsync(id)
            ?? throw new PdfNotFoundException();
     
        return await SetFavorite(userId, pdf);
    }

    public async Task<IList<UserPdf>> GetByTitleAsync(Guid userId, string title, IList<string>? tags = null)
    {
        var pdfs = await _pdfRepository.GetByTitleAsync(title, tags);
        return await SetFavorites(userId, pdfs);
    }

    public async Task<UserPdf> UpdateAsync(
        Guid userId,
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

            return await SetFavorite(userId, updated);
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

    public async Task<IList<Tag>> GetTagsAsync()
    {
        return (await _pdfRepository.GetAllTagsAsync()).ToList();
    }

    public async Task SaveProgressAsync(Guid userId, int pdfId, int currentPage)
    {
        await _pdfRepository.SaveProgressAsync(userId, pdfId, currentPage);
    }

    public async Task<IList<Progress>> GetProgressesAsync(Guid userId, int pdfId)
    {
        return (await _pdfRepository.GetProgressesAsync(userId, pdfId)).ToList();
    }

    public async Task AddToFavorites(Guid userId, int pdfId)
    {
        await _pdfRepository.AddToFavorites(userId, pdfId);
    }

    public async Task<IList<UserPdf>> GetUserFavoritePdfs(Guid userId, string? title = null, IEnumerable<string>? tags = null)
    {
        var pdfs = (await _pdfRepository.GetUserFavoritePdfs(userId, title, tags))
            .Select(p =>
            {
                var model = p.ToModel();
                model.IsFavorite = true;

                return model;
            }).ToList();
        
        return pdfs;
    }

    public async Task RemoveFromFavorites(Guid userId, int pdfId)
    {
        await _pdfRepository.RemoveFromFavorites(userId, pdfId);
    }

    private async Task<IList<UserPdf>> SetFavorites(Guid userId, IList<Pdf> pdfs)
    {
        var favoritePdfs = await _pdfRepository.GetUserFavoritePdfs(userId);
        return pdfs.Select(p =>
        {
            var model = p.ToModel();
            model.IsFavorite = favoritePdfs.Any(f => f.Id == p.Id);
            return model;
        }).ToList();
    }

    private async Task<UserPdf> SetFavorite(Guid userId, Pdf pdf)
    {
        var favoritePdfs = await _pdfRepository.GetUserFavoritePdfs(userId);
        var model = pdf.ToModel();
        model.IsFavorite = favoritePdfs.Any(f => f.Id == pdf.Id);
        
        return model;
    }
}
