using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Infrastructure.Data;

namespace FinancialSystem.Application;

public class UserMenu
{
    private readonly AppDbContext _context;

    public UserMenu(AppDbContext context)
    {
        _context = context;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление пользователями ===");
            Console.WriteLine("1. Создать пользователя");
            Console.WriteLine("2. Список пользователей");
            Console.WriteLine("3. Найти пользователя");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    //await ListUsersAsync();
                    break;
                case "3":
                    //await SearchUserAsync();
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

    private async Task CreateUserAsync()
    {
        Console.Write("ФИО: ");
        var name = Console.ReadLine();
        
        Console.Write("Email: ");
        var email = Console.ReadLine();
        
        Console.Write("Роль (0-Client, 1-Operator, 2-Manager, 3-Administrator): ");
        var role = (UserRole)int.Parse(Console.ReadLine());

        var user = new User
        {
            Name = name,
            Email = email,
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        Console.WriteLine("Пользователь создан!");
        Console.ReadKey();
    }

    // Остальные методы...
}