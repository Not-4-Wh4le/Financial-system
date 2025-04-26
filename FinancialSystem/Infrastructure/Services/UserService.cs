using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Services;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Enums;

namespace FinancialSystem.Infrastructure.Services;

 public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationService _authorizationService;

        public UserService(
            IUserRepository userRepository,
            IAuthorizationService authorizationService)
        {
            _userRepository = userRepository;
            _authorizationService = authorizationService;
        }

        public async Task<User> RegisterUserAsync(User executor, User newUser)
        {
            // Проверка прав: только администратор или менеджер может создавать пользователей
            if (!_authorizationService.CheckPermission(executor, Permission.ManageClients))
                throw new UnauthorizedAccessException("Недостаточно прав для создания пользователя");

            // Проверка уникальности email
            var existingUser = await _userRepository.GetByEmailAsync(newUser.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            await _userRepository.AddAsync(newUser);
            return newUser;
        }

        public async Task UpdateUserAsync(User executor, User updatedUser)
        {
            // Пользователь может редактировать себя сам, администратор - любого
            if (executor.Id != updatedUser.Id && 
                !_authorizationService.CheckPermission(executor, Permission.ManageClients))
            {
                throw new UnauthorizedAccessException("Недостаточно прав для редактирования пользователя");
            }

            var existingUser = await _userRepository.GetByIdAsync(updatedUser.Id);
            if (existingUser == null) throw new KeyNotFoundException("Пользователь не найден");

            // Обновление полей
            existingUser.Name = updatedUser.Name;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.Email = updatedUser.Email;
            existingUser.IsForeigner = updatedUser.IsForeigner;

            // Только администратор может менять роль
            if (executor.Role == UserRole.Administrator)
            {
                existingUser.Role = updatedUser.Role;
            }

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<User?> GetUserByIdAsync(User executor, int userId)
        {
            // Проверка прав: пользователь может смотреть себя, администратор - любого
            if (executor.Id != userId && 
                !_authorizationService.CheckPermission(executor, Permission.ViewAllLogs))
            {
                throw new UnauthorizedAccessException("Недостаточно прав для просмотра данных");
            }

            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task DeleteUserAsync(User executor, int userId)
        {
            if (!_authorizationService.CheckPermission(executor, Permission.ManageClients))
                throw new UnauthorizedAccessException("Недостаточно прав для удаления пользователя");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Пользователь не найден");

            await _userRepository.DeleteAsync(user);
        }

        public async Task<List<User>> SearchUsersAsync(User executor, string searchTerm)
        {
            if (!_authorizationService.CheckPermission(executor, Permission.ViewAllLogs))
                throw new UnauthorizedAccessException("Недостаточно прав для поиска пользователей");

            return await _userRepository.SearchUsersAsync(searchTerm);
        }

        public Task<List<User>> GetAllUserAsync(User executor)
        {
            if (!_authorizationService.CheckPermission(executor, Permission.ManageClients))
                throw new UnauthorizedAccessException("Недостаточно прав для просмотра пользователей");
            return _userRepository.GetAllAsync();
        }
    }
