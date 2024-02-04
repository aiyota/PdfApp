using Microsoft.EntityFrameworkCore;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;

namespace PdfApp.Infrastructure.Persistence.Repositories;

public class PdfRepository : IPdfRepository
{
    private readonly AppDbContext _dbContext;

    public PdfRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Pdf?> CreateAsync(
        string title,
        string? description,
        string? author,
        int totalPages,
        string fileName,
        IEnumerable<Tag> tags,
        bool? hasFile)
    {
        var tagsToAdd = await CreateTagListAsync(tags);
        var pdf = new Pdf
        {
            Title = title,
            Description = description,
            Author = author,
            TotalPages = totalPages,
            FileName = fileName,
            Tags = tagsToAdd,
            HasFile = hasFile ?? false,
            CreatedOn = DateTime.Now
        };



        var result = await _dbContext.Pdfs.AddAsync(pdf);
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        var pdf = await _dbContext.Pdfs.FindAsync(id);
        if (pdf is null)
            return;
        _dbContext.Pdfs.Remove(pdf);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<Pdf>> GetAllAsync()
    {
        return await _dbContext.Pdfs
            .Include(p => p.Tags)
            .ToListAsync();
    }

    public async Task<Pdf?> GetByIdAsync(int id)
    {
        return await _dbContext.Pdfs
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IList<Pdf>> GetByTitleAsync(string title)
    {
        return await _dbContext.Pdfs
            .Include(p => p.Tags)
            .Where(x => EF.Functions.Like(x.Title, $"%{title}%"))
            .ToListAsync();
    }

    public async Task<Pdf> UpdateAsync(
        int id,
        string? title = null,
        string? description = null,
        string? author = null,
        int? totalPages = null,
        string? fileName = null,
        IEnumerable<Tag>? tags = null,
        bool? hasFile = null
        )
    {
        var pdf = await _dbContext.Pdfs
           .Include(p => p.Tags)
           .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException($"Pdf with id {id} not found.");

        pdf.Title = title ?? pdf.Title;
        pdf.Description = description ?? pdf.Description;
        pdf.Author = author ?? pdf.Author;
        pdf.TotalPages = totalPages ?? pdf.TotalPages;
        pdf.FileName = fileName ?? pdf.FileName;
        pdf.Tags = tags?.ToList() ?? pdf.Tags;
        pdf.HasFile = hasFile ?? pdf.HasFile;

        await _dbContext.SaveChangesAsync();

        return pdf;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return await _dbContext.Tags.ToListAsync();
    }

    /// <summary>
    /// Creates a list of tags to add to the database taking the existing tags into consideration.
    /// </summary>
    private async Task<IList<Tag>> CreateTagListAsync(IEnumerable<Tag> tags)
    {
        IList<Tag> tagsToAdd = new List<Tag>();
        foreach (var tag in tags)
        {
            var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);
            tagsToAdd.Add(existingTag ?? tag);
        }

        return tagsToAdd;
    }

}
