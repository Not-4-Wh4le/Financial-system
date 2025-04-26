using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace FinancialSystem.Application;

public class BankMenu
{
    private readonly IBankService _bankService;
    private readonly IUserService _userService;
    private readonly IEnterpriseService _enterpriseService;
    private readonly IAuthorizationService _authorizationService;

    public BankMenu(
        IBankService bankService,
        IUserService userService,
        IEnterpriseService enterpriseService,
        IAuthorizationService authorizationService)
    {
        _bankService = bankService;
        _userService = userService;
        _enterpriseService = enterpriseService;
        _authorizationService = authorizationService;
    }

    public async Task ShowAsync(User currentUser)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Управление банками ({currentUser.Name}, {currentUser.Role}) ===");
            Console.WriteLine("1. Создать банк");
            Console.WriteLine("2. Список банков");
            Console.WriteLine("3. Добавить клиента в банк");
            Console.WriteLine("4. Добавить предприятие в банк");
            Console.WriteLine("5. Просмотреть клиентов банка");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateBankAsync(currentUser);
                        break;
                    case "2":
                        await ListBanksAsync(currentUser);
                        break;
                    case "3":
                        await AddClientToBankAsync(currentUser);
                        break;
                    case "4":
                        await AddEnterpriseToBankAsync(currentUser);
                        break;
                    case "5":
                        await ShowBankClientsAsync(currentUser);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private async Task CreateBankAsync(User executor)
    {
        Console.Write("Название банка: ");
        var name = Console.ReadLine();
        
        Console.Write("БИК: ");
        var bic = Console.ReadLine();

        var bank = new Bank
        {
            Name = name,
            Bic = bic
        };

        var createdBank = await _bankService.CreateBankAsync(executor, bank);
        Console.WriteLine($"Банк {createdBank.Name} успешно создан (ID: {createdBank.Id})!");
    }

    private async Task ListBanksAsync(User executor)
    {
        var banks = await _bankService.GetAllBanksAsync(executor);
        
        Console.WriteLine("Список банков:");
        Console.WriteLine("ID\tНазвание\tБИК");
        foreach (var bank in banks)
        {
            Console.WriteLine($"{bank.Id}\t{bank.Name}\t{bank.Bic}");
        }
    }

    private async Task AddClientToBankAsync(User executor)
    {
        // Показываем список банков
        var banks = await _bankService.GetAllBanksAsync(executor);
        Console.WriteLine("Доступные банки:");
        foreach (var bank in banks)
        {
            Console.WriteLine($"{bank.Id}. {bank.Name}");
        }
        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());

        // Показываем список пользователей
        var users = await _userService.GetAllUserAsync(executor);
        Console.WriteLine("Доступные пользователи:");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}. {user.Name} ({user.Email})");
        }
        Console.Write("ID пользователя: ");
        var userId = int.Parse(Console.ReadLine());

        await _bankService.RegisterClientAsync(executor, bankId, userId);
        Console.WriteLine("Клиент успешно добавлен в банк!");
    }

    private async Task AddEnterpriseToBankAsync(User executor)
    {
        var banks = await _bankService.GetAllBanksAsync(executor);
        Console.WriteLine("Доступные банки:");
        foreach (var bank in banks)
        {
            Console.WriteLine($"{bank.Id}. {bank.Name}");
        }
        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());

        var enterprises = await _enterpriseService.GetAllEnterprisesAsync(executor);
        Console.WriteLine("Доступные предприятия:");
        foreach (var enterprise in enterprises)
        {
            Console.WriteLine($"{enterprise.Id}. {enterprise.LegalName}");
        }
        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        await _bankService.RegisterEnterpriseAsync(executor, bankId, enterpriseId);
        Console.WriteLine("Предприятие успешно добавлено в банк!");
    }

    private async Task ShowBankClientsAsync(User executor)
    {
        var banks = await _bankService.GetAllBanksAsync(executor);
        Console.WriteLine("Доступные банки:");
        foreach (var bank in banks)
        {
            Console.WriteLine($"{bank.Id}. {bank.Name}");
        }
        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());

        var clients = await _bankService.GetBankClientsAsync(executor, bankId);
        
        Console.WriteLine("Клиенты банка:");
        Console.WriteLine("Тип\tID\tИмя/Название");
        foreach (var client in clients)
        {
            var type = client is User ? "Физ.лицо" : "Предприятие";
            Console.WriteLine($"{type}\t{client.Id}\t{client.Name}");
        }
    }
}