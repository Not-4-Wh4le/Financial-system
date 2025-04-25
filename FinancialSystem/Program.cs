using System;
using FinancialSystem.Application;
using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Servicec;
using FinancialSystem.Core.Interfaces.Services;
using FinancialSystem.Infrastructure.Data;
using FinancialSystem.Infrastructure.Repositories;
using FinancialSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        // Настройка DI контейнера
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        // Инициализация базы данных
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.SeedInitialDataAsync();
        }

        // Запуск главного меню
        var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
        await mainMenu.ShowAsync();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlite("Data Source=financial.db"));
        
        
        services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
    
        // Регистрация сервисов
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEnterpriseService, EnterpriseService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ILoanService,LoanService>();
       
        
        
        services.AddScoped<MainMenu>();
        services.AddScoped<UserMenu>();
        services.AddScoped<BankMenu>();
        services.AddScoped<EnterpriseMenu>();
        services.AddScoped<AccountMenu>();
        services.AddScoped<LoginMenu>();
    }
}