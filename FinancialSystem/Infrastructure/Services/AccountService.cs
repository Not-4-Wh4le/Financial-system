using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Servicec;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionService _transactionService;
    private readonly IAuthorizationService _authorizationService;

    public AccountService(
        IAccountRepository accountRepository,
        ITransactionService transactionService,
        IAuthorizationService authorizationService)
    {
        _accountRepository = accountRepository;
        _transactionService = transactionService;
        _authorizationService = authorizationService;
    }

    public async Task<decimal> GetBalanceAsync(User user, int accountId)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для просмотра баланса");

        var account = await _accountRepository.GetByIdAsync(accountId);
        return account?.Balance ?? throw new Exception("Счет не найден");
    }

    public async Task DepositAsync(User user, int accountId, decimal amount)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для пополнения счета");

        var account = await _accountRepository.GetByIdAsync(accountId);
        account.Balance += amount;
        await _accountRepository.UpdateAsync(account);
        
        await _transactionService.LogTransactionAsync(
            user, 
            null, 
            accountId, 
            amount, 
            TransactionType.Deposit,
            "Создание депозита");
    }

    public async Task WithdrawAsync(User user, int accountId, decimal amount)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для снятия средств");

        var account = await _accountRepository.GetByIdAsync(accountId);
        
        if(account.IsFrozen) 
            throw new InvalidOperationException("Счет заблокирован");
        
        if (account.Balance < amount)
            throw new InvalidOperationException("Недостаточно средств на счете");
            
        account.Balance -= amount;
        await _accountRepository.UpdateAsync(account);
        
        await _transactionService.LogTransactionAsync(
            user,
            accountId, 
            null, 
            amount, 
            TransactionType.Withdrawal,
            "Снятие средств");
    }

    public async Task TransferAsync(User user, int fromAccountId, int toAccountId, decimal amount)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для перевода средств");

        var fromAccount = await _accountRepository.GetByIdAsync(fromAccountId);
        var toAccount = await _accountRepository.GetByIdAsync(toAccountId);

        if (fromAccount.IsFrozen || toAccount.IsFrozen)
            throw new InvalidOperationException("Один из счетов заблокирован");

        if (fromAccount.Balance < amount)
            throw new InvalidOperationException("Недостаточно средств на счете");
            
        fromAccount.Balance -= amount;
        toAccount.Balance += amount;
        
        await _accountRepository.UpdateAsync(fromAccount);
        await _accountRepository.UpdateAsync(toAccount);
        
        await _transactionService.LogTransactionAsync(
            user,
            fromAccountId, 
            toAccountId, 
            amount, 
            TransactionType.Transfer,
            "Перевод средств");
    }

    public async Task FreezeAccountAsync(User user, int accountId)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageClients))
            throw new UnauthorizedAccessException("Недостаточно прав для блокировки счета");

        var account = await _accountRepository.GetByIdAsync(accountId);
        account.IsFrozen = true;
        await _accountRepository.UpdateAsync(account);
    }

    public async Task UnfreezeAccountAsync(User user, int accountId)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageClients))
            throw new UnauthorizedAccessException("Недостаточно прав для разблокировки счета");

        var account = await _accountRepository.GetByIdAsync(accountId);
        account.IsFrozen = false;
        await _accountRepository.UpdateAsync(account);
    }
    public async Task<AccountBase> CreateAccountAsync(User executor, AccountBase account)
    {
        // Проверка прав
        if (!_authorizationService.CheckPermission(executor, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для создания счета");

        if (account is UserAccount userAccount)
        {
            // Для личного счета проверяем, что владелец - текущий пользователь
            if (userAccount.Owner?.Id != executor.Id && 
                !_authorizationService.CheckPermission(executor, Permission.ManageClients))
            {
                throw new UnauthorizedAccessException("Недостаточно прав для создания счета другому пользователю");
            }
        }
        else if (account is EnterpriseAccount enterpriseAccount)
        {
            // Для корпоративного счета проверяем права на предприятие
            if (!_authorizationService.CheckPermission(executor, Permission.ManageEnterprises))
                throw new UnauthorizedAccessException("Недостаточно прав для создания корпоративного счета");
        }

        // Устанавливаем начальный баланс (если не установлен)
        account.Balance = account.Balance < 0 ? 0 : account.Balance;
        account.IsFrozen = false;

        // Сохраняем счет
        await _accountRepository.AddAsync(account);
    
        // Логируем создание счета
        await _transactionService.LogTransactionAsync(
            executor,
            null,
            account.Id,
            account.Balance,
            TransactionType.Deposit,
            $"Создан счет {account.Id}");

        return account;
    }
}