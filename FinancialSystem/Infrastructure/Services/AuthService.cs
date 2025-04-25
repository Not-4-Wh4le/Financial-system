using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationService _authorizationService;
    private User? _currentUser;

    public AuthService(
        IUserRepository userRepository,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _authorizationService = authorizationService;
    }

    public User? CurrentUser => _currentUser;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            _currentUser = null;
            return false;
        }

        _currentUser = user;
        return true;
    }

    public void Logout()
    {
        _currentUser = null;
    }

    public bool HasPermission(Permission permission)
    {
        if (_currentUser == null) return false;
        return _authorizationService.CheckPermission(_currentUser, permission);
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        return inputPassword == storedHash;
    }
}