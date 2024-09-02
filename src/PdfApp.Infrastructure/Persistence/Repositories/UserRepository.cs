using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;

namespace PdfApp.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(
        Guid id,
        string userName,
        string email)
    {
        var newUser = new User
        {
            Id = id,
            UserName = userName,
            Email = email,
            CreatedAt = DateTime.Now
        };

        _dbContext.Users.Add(newUser);

        await _dbContext.SaveChangesAsync();

        return newUser;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
       return await _dbContext.Users.FindAsync(id);
    }
}
