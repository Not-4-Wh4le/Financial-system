using FinancialSystem.Application;
using FinancialSystem.Core.Interfaces.Services;
using FinancialSystem.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

public class MainMenu
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAuthService _authService;

    public MainMenu(IServiceProvider serviceProvider, IAuthService authService)
    {
        _serviceProvider = serviceProvider;
        _authService = authService;
    }

    public async Task ShowAsync()
    {
        var loginMenu = _serviceProvider.GetRequiredService<LoginMenu>();
        var currentUser = await loginMenu.ShowAsync();
        
        if (currentUser == null) return;
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Банковская система ({currentUser.Name}, {currentUser.Role}) ===");
            Console.WriteLine("1. Управление пользователями");
            Console.WriteLine("2. Управление банками");
            Console.WriteLine("3. Управление предприятиями");
            Console.WriteLine("4. Управление счетами");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите раздел: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    //await _serviceProvider.GetRequiredService<UserMenu>().ShowAsync(currentUser);
                    break;
                case "2":
                    //await _serviceProvider.GetRequiredService<BankMenu>().ShowAsync(currentUser);
                    break;
                case "3":
                    //await _serviceProvider.GetRequiredService<EnterpriseMenu>().ShowAsync(currentUser);
                    break;
                case "4":
                   // await _serviceProvider.GetRequiredService<AccountMenu>().ShowAsync(currentUser);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}