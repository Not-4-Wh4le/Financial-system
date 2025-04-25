using Microsoft.EntityFrameworkCore;
using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
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
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);
        });

        // Конфигурация Enterprise
        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LegalName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.UNP).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.UNP).IsUnique();
            
            // Связь с Bank
            entity.HasOne(e => e.Bank)
                .WithMany()
                .HasForeignKey("BankId") // Явное указание имени поля
                .IsRequired();
        });

        // Конфигурация Bank
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Bic).IsRequired().HasMaxLength(20);
            entity.HasIndex(b => b.Bic).IsUnique();
           
            // Настройка для Clients (IBankClient)
            // Создаем две отдельные связи для User и Enterprise
            entity.HasMany<User>("ClientUsers")
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "BankUsers",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Bank>().WithMany().HasForeignKey("BankId"),
                    j => j.ToTable("BankUsers"));

            // Связь с Enterprise (юридические лица)
            entity.HasMany<Enterprise>("ClientEnterprises")
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "BankEnterprises",
                    j => j.HasOne<Enterprise>().WithMany().HasForeignKey("EnterpriseId"),
                    j => j.HasOne<Bank>().WithMany().HasForeignKey("BankId"),
                    j => j.ToTable("BankEnterprises"));
        });

        // Конфигурация Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            
            entity.HasOne(t => t.FromAccountBase)
                .WithMany()
                .HasForeignKey("FromAccountId")
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(t => t.ToAccountBase)
                .WithMany()
                .HasForeignKey("ToAccountId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация Loan
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Amount).HasColumnType("decimal(18,2)");
            
            entity.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey("UserId")
                .IsRequired();
        });

        // Настройка связи многие-ко-многим для EmployeeEnterprise
        modelBuilder.Entity<EmployeeEnterprise>()
            .HasKey(ee => new { ee.UserId, ee.EnterpriseId });
            
        modelBuilder.Entity<EmployeeEnterprise>()
            .HasOne(ee => ee.User)
            .WithMany(u => u.EmployedAt)
            .HasForeignKey(ee => ee.UserId);

        modelBuilder.Entity<EmployeeEnterprise>()
            .HasOne(ee => ee.Enterprise)
            .WithMany(e => e.Employees)
            .HasForeignKey(ee => ee.EnterpriseId);
    }

    public async Task SeedInitialDataAsync()
    {
        if (!await Banks.AnyAsync())
        {
            var banks = new List<Bank>
            {
                new Bank { 
                    Id = 1,
                    Name = "Альфа-Банк", 
                    Bic = "ALFABY2X",
                    Clients = new List<IBankClient>(),
                },
                new Bank { 
                    Id = 2,
                    Name = "Беларусбанк", 
                    Bic = "BELBBY2X",
                    Clients = new List<IBankClient>(),
                },
                new Bank { 
                    Id = 3,
                    Name = "Приорбанк", 
                    Bic = "PJCBBY2X",
                    Clients = new List<IBankClient>(),
                }
            };
            await Banks.AddRangeAsync(banks);
            await SaveChangesAsync();
        }

        if (!await Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User { 
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@bank.com",
                    Role = UserRole.Administrator,
                    PassportNumber = "AB1234567",
                    IdentificationNumber = "12345678901234"
                },
                new User { 
                    Id = 2,
                    Name = "Manager",
                    Email = "manager@bank.com",
                    Role = UserRole.Manager,
                    PassportNumber = "AB7654321",
                    IdentificationNumber = "98765432109876"
                }
            };
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();
        }
    }
}