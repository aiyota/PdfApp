using Microsoft.EntityFrameworkCore;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using System.Globalization;

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
        //pdf.Tags = tags?.ToList() ?? pdf.Tags;
        pdf.HasFile = hasFile ?? pdf.HasFile;

        await AddTagsToPdfAsync(tags, pdf);

        await _dbContext.SaveChangesAsync();

        return pdf;
    }

    private async Task AddTagsToPdfAsync(IEnumerable<Tag>? tags, Pdf pdf)
    {
        var tagNames = tags?.Select(t => t.Name.ToLower()).ToList();
        if (tagNames != null)
        {
            var existingTags = await _dbContext.Tags
                .Where(t => tagNames.Contains(t.Name.ToLower()))
                .ToListAsync();

            var newTagNames = tagNames.Except(existingTags.Select(t => t.Name.ToLower())).ToList();

            // for title casing
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var newTags = newTagNames
                .Select(name => new Tag { Name = textInfo.ToTitleCase(name) })
                .ToList();
            _dbContext.Tags.AddRange(newTags);

            // Combine existing and new tags
            var allTags = existingTags.Concat(newTags).ToList();

            var currentTags = pdf.Tags.ToList();

            // Remove tags that are no longer associated
            var tagsToRemove = currentTags.Except(allTags).ToList();
            foreach (var tag in tagsToRemove)
            {
                _dbContext.Entry(pdf).Collection(p => p.Tags).Load(); // Ensure the collection is loaded
                ((ICollection<Tag>)pdf.Tags).Remove(tag);
            }

            // Add new tags
            var tagsToAdd = allTags.Except(currentTags).ToList();
            foreach (var tag in tagsToAdd)
            {
                _dbContext.Entry(pdf).Collection(p => p.Tags).Load(); // Ensure the collection is loaded
                ((ICollection<Tag>)pdf.Tags).Add(tag);
            }
        }
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return await _dbContext.Tags
            .Include(t => t.Pdf)
            .ToListAsync();
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

    /// <summary>
    /// Saves the progress of the user reading a pdf.
    /// If the progress already exists, it updates the page number.
    /// </summary>
    public async Task SaveProgressAsync(Guid userId, int pdfId, int currentPage)
    {
        var progress = await _dbContext.Progresses
            .FirstOrDefaultAsync(p => p.PdfId == pdfId && p.UserId == userId);

        if (progress is not null)
        {
            progress.Page = currentPage;
        }
        else
        {
            progress = new Progress
            {
                Name = "Default",
                UserId = userId,
                PdfId = pdfId,
                Page = currentPage
            };
            _dbContext.Progresses.Add(progress);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Progress>> GetProgressesAsync(Guid userId, int pdfId)
    {
        return await _dbContext.Progresses
            .Where(p => p.UserId == userId && p.PdfId == pdfId)
            .ToListAsync();
    }
}
