using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;
using FinancialSystem.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Servicec;

namespace FinancialSystem.Application;

public class AccountMenu
{
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly AppDbContext _context;

    public AccountMenu(
        IAccountService accountService,
        IUserService userService,
        ITransactionService transactionService,
        AppDbContext context)
    {
        _accountService = accountService;
        _userService = userService;
        _transactionService = transactionService;
        _context = context;
    }

    public async Task ShowAsync(User currentUser)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Управление счетами ({currentUser.Name}) ===");
            Console.WriteLine("1. Создать счет");
            Console.WriteLine("2. Пополнить счет");
            Console.WriteLine("3. Перевести средства");
            Console.WriteLine("4. История операций");
            Console.WriteLine("5. Блокировка/разблокировка счета");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateAccountAsync(currentUser);
                        break;
                    case "2":
                        await DepositAsync(currentUser);
                        break;
                    case "3":
                        await TransferAsync(currentUser);
                        break;
                    case "4":
                        await ShowTransactionHistoryAsync(currentUser);
                        break;
                    case "5":
                        await ToggleAccountStatusAsync(currentUser);
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

    private async Task CreateAccountAsync(User executor)
    {
        Console.WriteLine("Тип счета:");
        Console.WriteLine("1. Личный счет");
        Console.WriteLine("2. Корпоративный счет");
        var accountTypeChoice = Console.ReadLine();

        AccountBase account;
        if (accountTypeChoice == "1")
        {
            account = new UserAccount
            {
                Owner = executor,
                AccountType = AccountType.Regular
            };
        }
        else
        {
            Console.Write("ID предприятия: ");
            var enterpriseId = int.Parse(Console.ReadLine());
            var enterprise = await _context.Enterprises.FindAsync(enterpriseId);
            
            account = new EnterpriseAccount
            {
                EnterpriseOwner = enterprise,
                IsMainAccount = false
            };
        }

        Console.Write("ID банка: ");
        var bankId = int.Parse(Console.ReadLine());
        var bank = await _context.Banks.FindAsync(bankId);
        account.Bank = bank;

        await _accountService.CreateAccountAsync(executor, account);
        Console.WriteLine("Счет успешно создан!");
    }

    private async Task DepositAsync(User executor)
    {
        Console.Write("ID счета: ");
        var accountId = int.Parse(Console.ReadLine());
        
        Console.Write("Сумма пополнения: ");
        var amount = decimal.Parse(Console.ReadLine());

        await _accountService.DepositAsync(executor, accountId, amount);
        Console.WriteLine("Счет успешно пополнен!");
    }

    private async Task TransferAsync(User executor)
    {
        Console.Write("ID счета отправителя: ");
        var fromAccountId = int.Parse(Console.ReadLine());
        
        Console.Write("ID счета получателя: ");
        var toAccountId = int.Parse(Console.ReadLine());
        
        Console.Write("Сумма перевода: ");
        var amount = decimal.Parse(Console.ReadLine());

        await _accountService.TransferAsync(executor, fromAccountId, toAccountId, amount);
        Console.WriteLine("Перевод выполнен успешно!");
    }

    private async Task ShowTransactionHistoryAsync(User executor)
    {
        Console.Write("ID счета: ");
        var accountId = int.Parse(Console.ReadLine());

        var transactions = await _transactionService.GetAccountTransactionsAsync(accountId);
        
        Console.WriteLine("История операций:");
        Console.WriteLine("Дата\t\tТип\tСумма\tСтатус");
        foreach (var t in transactions)
        {
            Console.WriteLine($"{t.Date:dd.MM.yy}\t{t.Type}\t{t.Amount}\t{t.Status}");
        }
    }

    private async Task ToggleAccountStatusAsync(User executor)
    {
        Console.Write("ID счета: ");
        var accountId = int.Parse(Console.ReadLine());

        var account = await _context.Accounts.FindAsync(accountId);
        if (account.IsFrozen)
        {
            await _accountService.UnfreezeAccountAsync(executor, accountId);
            Console.WriteLine("Счет разблокирован");
        }
        else
        {
            await _accountService.FreezeAccountAsync(executor, accountId);
            Console.WriteLine("Счет заблокирован");
        }
    }
}