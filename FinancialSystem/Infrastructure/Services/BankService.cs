using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ITransactionService _transactionService;

    public BankService(
        IBankRepository bankRepository,
        IUserRepository userRepository,
        IEnterpriseRepository enterpriseRepository,
        IAuthorizationService authorizationService)
    {
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _enterpriseRepository = enterpriseRepository;
        _authorizationService = authorizationService;   
    }

    public async Task RegisterClientAsync(User executor, int bankId, int userId)
    {
        if (!_authorizationService.CheckPermission(executor, Permission.ManageClients))
            throw new UnauthorizedAccessException("Недостаточно прав для регистрации клиента");
        
        var bank = await _bankRepository.GetByIdAsync(bankId);
        if (bank != null)
        {
            await _bankRepository.AddClientToBankAsync(bankId, userId);
            //await _bankRepository.UpdateAsync(bank);
        }
    }

    public async Task RegisterEnterpriseAsync(User executor, int bankId, int enterpriseId)
    {
        
        // Привязываем предприятие к банку
        var bank = await _bankRepository.GetByIdAsync(bankId);
        if (bank != null)
        {
            await _bankRepository.AddEnterpriseToBankAsync(bankId, enterpriseId);
            //await _bankRepository.UpdateAsync(bank);
        }
    }
    
    public Task<Bank> GetBankInfoAsync(int bankId)
    {
        throw new NotImplementedException();
    }

    public Task AssignAccountToBankAsync(int bankId, int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Bank>> GetAllBanksAsync(User executor)
    {
        if (!_authorizationService.CheckPermission(executor, Permission.ViewStatistics))
            throw new UnauthorizedAccessException("Недостаточно прав для регистрации клиента");
        return _bankRepository.GetAllAsync();
    }

    public async Task<Bank> CreateBankAsync(User executor, Bank bank)
    {
        if (!_authorizationService.CheckPermission(executor, Permission.ManageEnterprises))
        {
            throw new UnauthorizedAccessException("Недостаточно прав для создания банка");
        }

        if (string.IsNullOrWhiteSpace(bank.Name))
        {
            throw new ArgumentException("Название банка не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(bank.Bic))
        {
            throw new ArgumentException("БИК банка не может быть пустым");
        }

        var newBank = new Bank
        {
            Name = bank.Name,
            Bic = bank.Bic,
            // Инициализация коллекций
            ClientUsers = new List<User>(),
            ClientEnterprises = new List<Enterprise>()
        };

        // 5. Сохранение в репозитории
        await _bankRepository.AddAsync(newBank);

        // 6. Логирование действия
        await _transactionService.LogTransactionAsync(
            executor,
            null,
            null,
            0,
            TransactionType.System,
            $"Создан новый банк: {newBank.Name} (БИК: {newBank.Bic})");

        return newBank;
    }

    public async Task<List<IBankClient>> GetBankClientsAsync(User executor, int bankId)
    {
        if (!_authorizationService.CheckPermission(executor, Permission.ManageClients))
        {
            throw new UnauthorizedAccessException("Недостаточно прав для просмотра клиентов банка");
        }

        var bank = await _bankRepository.GetByIdWithClientsAsync(bankId);
        if (bank == null)
        {
            throw new KeyNotFoundException("Банк не найден");
        }

        var clients = new List<IBankClient>();

        if (bank.ClientUsers != null)
        {
            clients.AddRange(bank.ClientUsers.Cast<IBankClient>());
        }

        if (bank.ClientEnterprises != null)
        {
            clients.AddRange(bank.ClientEnterprises.Cast<IBankClient>());
        }

        await _transactionService.LogTransactionAsync(
            executor,
            null,
            null,
            0,
            TransactionType.System,
            $"Просмотр клиентов банка ID: {bankId}");

        return clients;
    }
}