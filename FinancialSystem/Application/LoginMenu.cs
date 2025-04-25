using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Application;

public class LoginMenu
{
    private readonly IAuthService _authService;

    public LoginMenu(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<User?> ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Вход в систему ===");
        
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? string.Empty;
        
        Console.Write("Пароль: ");
        var password = ReadPassword(); // Скрытый ввод пароля

        if (await _authService.LoginAsync(email, password))
        {
            Console.WriteLine($"Добро пожаловать, {_authService.CurrentUser?.Name}!");
            return _authService.CurrentUser;
        }

        Console.WriteLine("Неверный email или пароль!");
        return null;
    }

    private string ReadPassword()
    {
        var password = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        Console.WriteLine();
        return password;
    }
}