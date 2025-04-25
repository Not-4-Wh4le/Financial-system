using FinancialSystem.Infrastructure.Data;

namespace FinancialSystem.Application;

using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class EnterpriseMenu
{
    private readonly AppDbContext _context;
    private readonly BankMenu _bankMenu;

    public EnterpriseMenu(AppDbContext context, BankMenu bankMenu)
    {
        _context = context;
        _bankMenu = bankMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление предприятиями ===");
            Console.WriteLine("1. Создать предприятие");
            Console.WriteLine("2. Список предприятий");
            Console.WriteLine("3. Добавить сотрудника");
            Console.WriteLine("4. Создать зарплатный проект");
            Console.WriteLine("5. Просмотреть счета предприятия");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateEnterpriseAsync();
                    break;
                case "2":
                    await ListEnterprisesAsync();
                    break;
                case "3":
                    await AddEmployeeAsync();
                    break;
                case "4":
                    await CreateSalaryProjectAsync();
                    break;
                case "5":
                    await ShowEnterpriseAccountsAsync();
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

    private async Task CreateEnterpriseAsync()
    {
        Console.Write("Юридическое название: ");
        var legalName = Console.ReadLine();

        Console.Write("УНП: ");
        var unp = Console.ReadLine();

        Console.Write("Юридический адрес: ");
        var address = Console.ReadLine();

        Console.WriteLine("Выберите тип предприятия:");
        Console.WriteLine("1. ИП\n2. ООО\n3. ЗАО\n4. Государственное\n5. Другое");
        var typeChoice = int.Parse(Console.ReadLine());
        var type = (EnterpriseType)(typeChoice - 1);

        // Выбор банка
       // await _bankMenu.ListBanksAsync();
        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());
        var bank = await _context.Banks.FindAsync(bankId);

        var enterprise = new Enterprise
        {
            LegalName = legalName,
            UNP = unp,
            LegalAddress = address,
            Type = type,
            Bank = bank
        };

        _context.Enterprises.Add(enterprise);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Предприятие {legalName} создано!");
        Console.ReadKey();
    }

    private async Task ListEnterprisesAsync()
    {
        var enterprises = await _context.Enterprises
            .Include(e => e.Bank)
            .ToListAsync();

        Console.WriteLine("Список предприятий:");
        Console.WriteLine("ID\tНазвание\tТип\tБИК банка");
        foreach (var e in enterprises)
        {
            Console.WriteLine($"{e.Id}\t{e.LegalName}\t{e.Type}\t{e.Bank?.Bic}");
        }
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    private async Task AddEmployeeAsync()
    {
        await ListEnterprisesAsync();
        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        var users = await _context.Users.ToListAsync();
        Console.WriteLine("Доступные пользователи:");
        foreach (var u in users)
        {
            Console.WriteLine($"{u.Id}\t{u.Name}\t{u.Email}");
        }

        Console.Write("ID пользователя: ");
        var userId = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите роль сотрудника:");
        Console.WriteLine("1. Сотрудник\n2. Специалист\n3. Бухгалтер");
        var roleChoice = int.Parse(Console.ReadLine());
        var role = (EnterpriseRole)(roleChoice - 1);

        var employee = new EmployeeEnterprise
        {
            EnterpriseId = enterpriseId,
            UserId = userId,
            Role = role
        };

        _context.EmployeeEnterprises.Add(employee);
        await _context.SaveChangesAsync();

        Console.WriteLine("Сотрудник добавлен!");
        Console.ReadKey();
    }

    private async Task CreateSalaryProjectAsync()
    {
        await ListEnterprisesAsync();
        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        var enterprise = await _context.Enterprises
            .Include(e => e.Employees)
            .ThenInclude(ee => ee.User)
            .FirstOrDefaultAsync(e => e.Id == enterpriseId);

        if (enterprise == null)
        {
            Console.WriteLine("Предприятие не найдено!");
            Console.ReadKey();
            return;
        }

        foreach (var employee in enterprise.Employees)
        {
            var account = new UserAccount
            {
                Owner = employee.User,
                Bank = enterprise.Bank,
                AccountType = AccountType.Salary,
                Balance = 0
            };
            _context.Accounts.Add(account);
        }

        await _context.SaveChangesAsync();
        Console.WriteLine($"Зарплатный проект создан для {enterprise.LegalName}!");
        Console.ReadKey();
    }

    private async Task ShowEnterpriseAccountsAsync()
    {
        await ListEnterprisesAsync();
        Console.Write("ID предприятия: ");
        var enterpriseId = int.Parse(Console.ReadLine());

        var accounts = await _context.Accounts
            .OfType<EnterpriseAccount>()
            .Where(a => a.EnterpriseOwner.Id == enterpriseId)
            .ToListAsync();

        Console.WriteLine("Счета предприятия:");
        foreach (var acc in accounts)
        {
            Console.WriteLine($"{acc.Id}\t{acc.Balance}\t{(acc.IsMainAccount ? "Основной" : "Дополнительный")}");
        }
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}