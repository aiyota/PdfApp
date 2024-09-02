using Microsoft.EntityFrameworkCore;
using PdfApp.Domain.Entities;

namespace PdfApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Pdf> Pdfs { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<Progress> Progresses { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.UserName).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.ModifiedAt).IsRequired(false);


        modelBuilder.Entity<Pdf>().HasKey(p => p.Id);
        modelBuilder.Entity<Pdf>().Property(p => p.Title).IsRequired();
        modelBuilder.Entity<Pdf>().Property(p => p.TotalPages).IsRequired();
        modelBuilder.Entity<Pdf>().Property(p => p.CreatedOn).IsRequired();
        modelBuilder.Entity<Pdf>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Pdf);

        modelBuilder.Entity<Tag>().HasKey(p => p.Id);
        modelBuilder.Entity<Tag>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Tag>().HasIndex(p => p.Name).IsUnique();

        modelBuilder.Entity<Progress>().HasKey(p => p.Id);
        modelBuilder.Entity<Progress>()
            .HasOne<Pdf>()
            .WithMany()
            .HasForeignKey(p => p.PdfId)
            .IsRequired();
        modelBuilder.Entity<Progress>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .IsRequired();
        modelBuilder.Entity<Progress>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Progress>().Property(p => p.Page).IsRequired();
    }
}
