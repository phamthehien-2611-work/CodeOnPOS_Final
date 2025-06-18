using UserManagementService.Application.Contracts.Persistence;
using UserManagementService.Domain.Entities;
using UserManagementService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;

    public UserRepository(UserDbContext context)
    {
        _dbContext = context;
    }

    public async Task<tbl_User> AddAsync(tbl_User user)
    {
        await _dbContext.tbl_Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    public async Task<tbl_User?> GetUserByUserNameAsync(string username)
    {
        return await _dbContext.tbl_Users.FirstOrDefaultAsync(u => u.UserName == username);
    }
    public async Task<tbl_User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.tbl_Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}