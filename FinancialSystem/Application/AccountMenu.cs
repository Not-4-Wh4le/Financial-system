using FinancialSystem.Infrastructure.Data;

namespace FinancialSystem.Application;

public class AccountMenu
{
    private readonly AppDbContext _context;

    public AccountMenu(AppDbContext context)
    {
        _context = context;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление счетами ===");
            Console.WriteLine("1. Создать счет");
            Console.WriteLine("2. Пополнить счет");
            Console.WriteLine("3. Перевести средства");
            Console.WriteLine("4. История операций");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateAccountAsync();
                    break;
                case "2":
                   // await DepositAsync();
                    break;
                case "3":
                   // await TransferAsync();
                    break;
                case "4":
                    //await ShowTransactionHistoryAsync();
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

    private async Task CreateAccountAsync()
    {
        // Реализация создания счета
    }

    // Остальные методы...
}