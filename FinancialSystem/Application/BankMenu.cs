using FinancialSystem.Core.Entities;
using FinancialSystem.Infrastructure.Data;


namespace FinancialSystem.Application;

public class BankMenu
{
    private readonly AppDbContext _context;

    public BankMenu(AppDbContext context)
    {
        _context = context;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление банками ===");
            Console.WriteLine("1. Создать банк");
            Console.WriteLine("2. Список банков");
            Console.WriteLine("3. Добавить клиента в банк");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateBankAsync();
                    break;
                case "2":
                    //await ListBanksAsync();
                    break;
                case "3":
                   // await AddClientToBankAsync();
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

    private async Task CreateBankAsync()
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

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync();
        
        Console.WriteLine("Банк создан!");
        Console.ReadKey();
    }

    // Остальные методы...
}