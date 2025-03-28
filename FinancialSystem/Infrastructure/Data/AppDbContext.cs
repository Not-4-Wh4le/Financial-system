using System.Data.Entity;
using FinancialSystem.Core.Entities;

namespace FinancialSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<AccountBase> Accounts { get; set; }
    
    
}