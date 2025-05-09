﻿using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsForeignerAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.IsForeigner ?? false;
    }

    public async Task<List<User>> GetEmployeesByEnterpriseAsync(int enterpriseId)
    {
        return await _context.EmployeeEnterprises
            .Where(ee => ee.EnterpriseId == enterpriseId)
            .Select(ee => ee.User)
            .ToListAsync();
    }
    
    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await _context.Users.ToListAsync();
        }

        var formattedSearchTerm = $"%{searchTerm.Trim()}%";

        var query = _context.Users
            .Where(u =>
                EF.Functions.Like(u.Name, formattedSearchTerm) ||
                EF.Functions.Like(u.Email, formattedSearchTerm) ||
                EF.Functions.Like(u.PassportNumber, formattedSearchTerm) ||
                EF.Functions.Like(u.PhoneNumber, formattedSearchTerm) ||
                EF.Functions.Like(u.IdentificationNumber, formattedSearchTerm));

        return await query.ToListAsync();
    }
}