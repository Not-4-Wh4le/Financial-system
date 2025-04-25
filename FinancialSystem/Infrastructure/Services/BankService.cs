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

    public async Task RegisterClientAsync(User user, User newUser)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageClients))
            throw new UnauthorizedAccessException("Недостаточно прав для регистрации клиента");

        
        if (newUser == null)
            throw new ArgumentNullException(nameof(newUser));
        
       await _userRepository.AddAsync(newUser);
        
        // Привязываем клиента к банку (если требуется)
        var bank = await _bankRepository.GetDefaultBankAsync();
        if (bank != null)
        {
            await _bankRepository.AddClientToBankAsync(bank.Id, user.Id);
            //await _bankRepository.UpdateAsync(bank);
        }
    }

    public async Task RegisterEnterpriseAsync(User user, Enterprise enterprise)
    {
        if (enterprise == null)
            throw new ArgumentNullException(nameof(enterprise));
        

        await _enterpriseRepository.AddAsync(enterprise);
        
        // Привязываем предприятие к банку
        var bank = await _bankRepository.GetDefaultBankAsync();
        if (bank != null)
        {
            
            await _bankRepository.AddEnterpriseToBankAsync(bank.Id, enterprise.Id);
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
}