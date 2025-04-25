using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;
using FinancialSystem.Core.Interfaces.Servicec;

public class EnterpriseMenu
{
    private readonly IEnterpriseService _enterpriseService;
    private readonly IBankService _bankService;
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;

    public EnterpriseMenu(
        IEnterpriseService enterpriseService,
        IBankService bankService,
        IUserService userService,
        IAccountService accountService)
    {
        _enterpriseService = enterpriseService;
        _bankService = bankService;
        _userService = userService;
        _accountService = accountService;
    }

    public async Task ShowAsync(User currentUser)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление предприятиями ===");
            Console.WriteLine("1. Зарегистрировать предприятие");
            Console.WriteLine("2. Список предприятий");
            Console.WriteLine("3. Добавить сотрудника");
            Console.WriteLine("4. Создать зарплатный проект");
            Console.WriteLine("5. Просмотреть счета предприятия");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        await RegisterEnterpriseAsync(currentUser);
                        break;
                    case "2":
                        await ListEnterprisesAsync(currentUser);
                        break;
                    case "3":
                        await AddEmployeeAsync(currentUser);
                        break;
                    case "4":
                        await CreateSalaryProjectAsync(currentUser);
                        break;
                    case "5":
                        await ShowEnterpriseAccountsAsync(currentUser);
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

    private async Task RegisterEnterpriseAsync(User executor)
    {
        Console.Write("Юридическое название: ");
        var legalName = Console.ReadLine();

        Console.Write("УНП: ");
        var unp = Console.ReadLine();

        // Показываем список банков
        var banks = await _bankService.GetAllBanksAsync(executor);
        Console.WriteLine("Доступные банки:");
        foreach (var bank in banks)
        {
            Console.WriteLine($"{bank.Id}. {bank.Name} ({bank.Bic})");
        }

        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());

        var enterprise = await _enterpriseService.RegisterEnterpriseAsync(
            executor,
            legalName,
            unp,
            bankId);

        Console.WriteLine($"Предприятие {enterprise.LegalName} зарегистрировано!");
    }

    private async Task ListEnterprisesAsync(User executor)
    {
        var enterprises = await _enterpriseService.GetAllEnterprisesAsync(executor);
        
        Console.WriteLine("Список предприятий:");
        Console.WriteLine("ID\tНазвание\tУНП\tБИК банка");
        foreach (var e in enterprises)
        {
            Console.WriteLine($"{e.Id}\t{e.LegalName}\t{e.UNP}\t{e.Bank.Bic}");
        }
    }

    private async Task AddEmployeeAsync(User executor)
    {
        var enterprises = await _enterpriseService.GetAllEnterprisesAsync(executor);
        Console.WriteLine("Доступные предприятия:");
        foreach (var e in enterprises)
        {
            Console.WriteLine($"{e.Id}. {e.LegalName}");
        }

        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        var users = await _userService.SearchUsersAsync(executor, "");
        Console.WriteLine("Доступные пользователи:");
        foreach (var u in users)
        {
            Console.WriteLine($"{u.Id}. {u.Name} ({u.Email})");
        }

        Console.Write("ID пользователя: ");
        var userId = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите роль:");
        Console.WriteLine("1. Сотрудник\n2. Специалист\n3. Бухгалтер");
        var role = (EnterpriseRole)(int.Parse(Console.ReadLine()) - 1);

        await _enterpriseService.AddEmployeeAsync(
            executor,
            enterpriseId,
            userId,
            role);

        Console.WriteLine("Сотрудник успешно добавлен!");
    }

    private async Task CreateSalaryProjectAsync(User executor)
    {
        var enterprises = await _enterpriseService.GetAllEnterprisesAsync(executor);
        Console.WriteLine("Доступные предприятия:");
        foreach (var e in enterprises)
        {
            Console.WriteLine($"{e.Id}. {e.LegalName}");
        }

        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        await _enterpriseService.CreateSalaryProjectAsync(executor, enterpriseId);
        Console.WriteLine("Зарплатный проект успешно создан!");
    }

    private async Task ShowEnterpriseAccountsAsync(User executor)
    {
        var enterprises = await _enterpriseService.GetAllEnterprisesAsync(executor);
        Console.WriteLine("Доступные предприятия:");
        foreach (var e in enterprises)
        {
            Console.WriteLine($"{e.Id}. {e.LegalName}");
        }

        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        //var accounts = await _enterpriseService.GetEnterpriseAccountsAsync(enterpriseId);
        
        Console.WriteLine("Счета предприятия:");/*
        foreach (var acc in accounts)
        {
            Console.WriteLine($"{acc.Id}: {acc.Balance} {acc.GetType().Name}");
        }*/
    }
}