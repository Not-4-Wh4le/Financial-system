using Microsoft.EntityFrameworkCore;
using FinancialSystem.Core.Entities;

namespace FinancialSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<AccountBase> Accounts { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<EnterpriseAccount> EnterpriseAccounts { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<EmployeeEnterprise> EmployeeEnterprises { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация наследования для AccountBase
        modelBuilder.Entity<AccountBase>()
            .HasDiscriminator<string>("AccountType")
            .HasValue<UserAccount>("UserAccount")
            .HasValue<EnterpriseAccount>("EnterpriseAccount");

        // Конфигурация User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.ID);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // Конфигурация Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasBaseType<User>();
        });

        // Конфигурация Enterprise
        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LegalName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.UNP).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.UNP).IsUnique();
        });

        // Конфигурация Bank
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Bic).IsRequired().HasMaxLength(20);
            entity.HasIndex(b => b.Bic).IsUnique();
        });

        // Конфигурация Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.HasOne(t => t.FromAccountBase)
                  .WithMany()
                  .HasForeignKey(t => t.FromAccountBase.Id)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.ToAccountBase)
                  .WithMany()
                  .HasForeignKey(t => t.ToAccountBase.Id)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация Loan
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Amount).HasColumnType("decimal(18,2)");
            entity.HasOne(l => l.Client)
                  .WithMany()
                  .HasForeignKey(l => l.Client.Id);
        });
        
        modelBuilder.Entity<EmployeeEnterprise>()
            .HasKey(ee => new { ee.UserId, ee.EnterpriseId });
    }
    public async Task SeedInitialDataAsync()
    {
        if (!await Banks.AnyAsync())
        {
            var banks = new List<Bank>
            {
                new Bank { Name = "Альфа-Банк", Bic = "ALFABY2X" },
                new Bank { Name = "Беларусбанк", Bic = "BELBBY2X" },
                new Bank { Name = "Приорбанк", Bic = "PJCBBY2X" }
            };
            await Banks.AddRangeAsync(banks);
        }

        

        await SaveChangesAsync();
    }
}