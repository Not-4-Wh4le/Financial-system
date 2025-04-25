using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

public class EnterpriseService : IEnterpriseService
{
    private readonly IEnterpriseRepository _enterpriseRepo;
    private readonly IUserRepository _userRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly AuthorizationService _authService;
    private readonly IBankRepository _bankRepo;

    public EnterpriseService(
        IEnterpriseRepository enterpriseRepo,
        IUserRepository userRepo,
        IAccountRepository accountRepo,
        AuthorizationService authService,
        IBankRepository bankRepo)
    {
        _enterpriseRepo = enterpriseRepo;
        _userRepo = userRepo;
        _accountRepo = accountRepo;
        _authService = authService;
        _bankRepo = bankRepo;
    }

    public async Task<Enterprise> RegisterEnterpriseAsync(User executor, string legalName, string unp, int bankId)
    {
        if (!_authService.CheckPermission(executor, Permission.ManageEnterprises))
            throw new UnauthorizedAccessException();

        // Проверка уникальности УНП
        /*if (await _enterpriseRepo.GetByUNPAsync(unp) != null)
            throw new InvalidOperationException("УНП должен быть уникальным");
            */

        var bank = await _bankRepo.GetByIdAsync(bankId);
        var enterprise = new Enterprise
        {
            LegalName = legalName,
            UNP = unp,
            Bank = bank
        };

        await _enterpriseRepo.AddAsync(enterprise);
        return enterprise;
    }

    public async Task AddEmployeeAsync(User executor, int enterpriseId, int userId, EnterpriseRole role)
    {
        if (!_authService.CheckPermission(executor, Permission.ManageSalaryProjects))
            throw new UnauthorizedAccessException();

        var enterprise = await _enterpriseRepo.GetByIdAsync(enterpriseId);
        var user = await _userRepo.GetByIdAsync(userId);

        enterprise.Employees.Add(new EmployeeEnterprise
        {
            User = user,
            Enterprise = enterprise,
            Role = role
        });

        await _enterpriseRepo.UpdateAsync(enterprise);
    }

    public async Task CreateSalaryProjectAsync(User executor, int enterpriseId)
    {
        if (!_authService.CheckPermission(executor, Permission.ManageSalaryProjects))
            throw new UnauthorizedAccessException();

        var enterprise = await _enterpriseRepo.GetByIdWithEmployeesAsync(enterpriseId);
        var employees = enterprise.Employees;

        // Создаем зарплатные счета для сотрудников
        foreach (var employee in employees)
        {
            var salaryAccount = new UserAccount
            {
                Owner = employee.User,
                Bank = enterprise.Bank,
                AccountType = AccountType.Salary
            };
            await _accountRepo.AddAsync(salaryAccount);
        }
    }

    public Task<List<Enterprise>> GetAllEnterprisesAsync(User executor)
    {
        if (!_authService.CheckPermission(executor, Permission.ManageEnterprises))
            throw new UnauthorizedAccessException();
        return _enterpriseRepo.GetAllAsync();
    }

    public async Task PaySalariesAsync(User executor, int enterpriseId, Dictionary<int, decimal> payments)
    {
        if (!_authService.CheckPermission(executor, Permission.ManageSalaryProjects))
            throw new UnauthorizedAccessException();

        var enterprise = await _enterpriseRepo.GetByIdAsync(enterpriseId);
        var mainAccount = await _enterpriseRepo.GetMainEnterpriseAccountAsync(enterpriseId)
                          ?? throw new InvalidOperationException("Основной счет не найден");

        foreach (var (employeeId, amount) in payments)
        {
            var employeeAccount = await _accountRepo.GetSalaryAccountAsync(employeeId);

            // Проверка баланса
            if (mainAccount.Balance < amount)
                throw new InvalidOperationException("Недостаточно средств");

            // Перевод средств
            mainAccount.Balance -= amount;
            employeeAccount.Balance += amount;

            await _accountRepo.UpdateAsync(mainAccount);
            await _accountRepo.UpdateAsync(employeeAccount);
        }
    }
} 