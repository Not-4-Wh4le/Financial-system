using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace FinancialSystem.Application;

public class UserMenu
{
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;

    public UserMenu(
        IUserService userService,
        IAuthorizationService authorizationService)
    {
        _userService = userService;
        _authorizationService = authorizationService;
    }

    public async Task ShowAsync(User currentUser)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Управление пользователями ({currentUser.Name}, {currentUser.Role}) ===");
            Console.WriteLine("1. Создать пользователя");
            Console.WriteLine("2. Список пользователей");
            Console.WriteLine("3. Найти пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateUserAsync(currentUser);
                        break;
                    case "2":
                        await ListUsersAsync(currentUser);
                        break;
                    case "3":
                        await SearchUserAsync(currentUser);
                        break;
                    case "4":
                        await DeleteUserAsync(currentUser);
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

    private async Task CreateUserAsync(User executor)
    {
        Console.Write("ФИО: ");
        var name = Console.ReadLine();
        
        Console.Write("Email: ");
        var email = Console.ReadLine();
        
        Console.Write("Номер паспорта: ");
        var passportNumber = Console.ReadLine();
        
        Console.Write("Идентификационный номер: ");
        var identificationNumber = Console.ReadLine();
        
        Console.Write("Телефон: ");
        var phoneNumber = Console.ReadLine();

        Console.WriteLine("Роли:");
        Console.WriteLine("0. Клиент");
        Console.WriteLine("1. Оператор");
        Console.WriteLine("2. Менеджер");
        Console.WriteLine("3. Администратор");
        Console.WriteLine("4. Специалист предприятия");
        Console.Write("Выберите роль: ");
        var role = (UserRole)int.Parse(Console.ReadLine());

        Console.Write("Пароль: ");
        var password = Console.ReadLine();

        var newUser = new User
        {
            Name = name,
            Email = email,
            PassportNumber = passportNumber,
            IdentificationNumber = identificationNumber,
            PhoneNumber = phoneNumber,
            Role = role,
            PasswordHash = password // В реальном приложении нужно хешировать пароль!
        };

        var createdUser = await _userService.RegisterUserAsync(executor, newUser);
        Console.WriteLine($"Пользователь {createdUser.Name} успешно создан (ID: {createdUser.Id})!");
    }

    private async Task ListUsersAsync(User executor)
    {
        var users = await _userService.GetAllUserAsync(executor);
        
        Console.WriteLine("Список пользователей:");
        Console.WriteLine("ID\tИмя\tEmail\tРоль");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}\t{user.Name}\t{user.Email}\t{user.Role}");
        }
    }

    private async Task SearchUserAsync(User executor)
    {
        Console.Write("Поиск (имя, email или паспорт): ");
        var searchTerm = Console.ReadLine();

        var users = await _userService.SearchUsersAsync(executor, searchTerm);
        
        Console.WriteLine("Результаты поиска:");
        Console.WriteLine("ID\tИмя\tEmail\tТелефон");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}\t{user.Name}\t{user.Email}\t{user.PhoneNumber}");
        }
    }

    private async Task DeleteUserAsync(User executor)
    {
        Console.Write("ID пользователя для удаления: ");
        var userId = int.Parse(Console.ReadLine());

        await _userService.DeleteUserAsync(executor, userId);
        Console.WriteLine("Пользователь успешно удален!");
    }
}