using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using CampusEats.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Persistence.Repositories;

public class UserRepository(CampusEatsDbContext dbContext)
{

    public async Task<User> CreateUserAsync(User user)
    {
        var existing = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());

        if (existing != null)
            throw new DuplicateEmailException(user.Email);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        var user = await dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new UserNotFoundException(userId);

        return user;
    }

    public async Task<User> GetUserByNameAsync(string firstName, string lastName)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.FirstName.ToLower() == firstName.ToLower() &&
                u.LastName.ToLower() == lastName.ToLower());

        if (user == null)
            throw new EntityNotFoundException("User", $"{firstName} {lastName}");

        return user;
    }

    public async Task<int> CountUsersByRoleAsync(string role)
    {
        return await dbContext.Users.CountAsync(u => u.Role == role);
    }

    public async Task<List<User>> GetUsersByRoleAsync(string role, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await dbContext.Users
            .Where(u => u.Role == role)
            .OrderByDescending(u => u.UserId)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task UpdateRefreshTokenAsync(int userId, string token, DateTime expiresAt)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            throw new UserNotFoundException(userId);

        user.RefreshToken = token;
        user.RefreshTokenExpiresAt = expiresAt;

        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.RefreshToken == refreshToken &&
                u.RefreshTokenExpiresAt > DateTime.UtcNow);
    }
}
