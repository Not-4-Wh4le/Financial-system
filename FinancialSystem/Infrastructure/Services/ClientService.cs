using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

 public class ClientService : IClientService
    {
        private readonly IUserRepository _userRepo;
        private readonly IBankRepository _bankRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IAuthorizationService _authService;
        private readonly ILoanRepository _loanRepo;

        public ClientService(
            IUserRepository userRepo,
            IBankRepository bankRepo,
            IAccountRepository accountRepo,
            IAuthorizationService authService,
            ILoanRepository loanRepo)
        {
            _userRepo = userRepo;
            _bankRepo = bankRepo;
            _accountRepo = accountRepo;
            _authService = authService;
            _loanRepo = loanRepo;
        }

        public async Task<Client> RegisterClientAsync(User executor, string fullName, string passport, string phone, int bankId)
        {
            // Проверка прав: только менеджер или администратор
            if (!_authService.CheckPermission(executor, Permission.ManageClients))
                throw new UnauthorizedAccessException("Недостаточно прав");

            // Создание User
            var user = new User
            {
                Name = fullName,
                PassportNumber = passport,
                PhoneNumber = phone,
                Role = UserRole.Client
            };

            await _userRepo.AddAsync(user);

            // Привязка к банку
            var bank = await _bankRepo.GetByIdAsync(bankId) 
                ?? throw new KeyNotFoundException("Банк не найден");
            
            var client = new Client { User = user };
            bank.Clients.Add(client);
            await _bankRepo.UpdateAsync(bank);

            return client;
        }

        public async Task FreezeClientAccountAsync(User executor, int accountId)
        {
            // Проверка прав: клиент может замораживать только свои счета
            var account = await _accountRepo.GetByIdAsync(accountId) as UserAccount 
                ?? throw new InvalidOperationException("Неверный тип счета");
            
            if (executor.Id != account.Owner.Id && 
                !_authService.CheckPermission(executor, Permission.ManageClients))
            {
                throw new UnauthorizedAccessException();
            }

            account.IsFrozen = true;
            await _accountRepo.UpdateAsync(account);
        }

        public async Task RequestLoanAsync(User executor, decimal amount, int months)
        {
            if (!_authService.CheckPermission(executor, Permission.RequestLoan))
                throw new UnauthorizedAccessException();

            var user = await _userRepo.GetByIdAsync(executor.Id) 
                       ?? throw new KeyNotFoundException("Пользователь не найден");

            var client = new Client { User = user };
            var loan = new Loan
            {
                User = user,
                Amount = amount,
                DurationMonths = months,
                IsApproved = false
            };

            await _loanRepo.AddAsync(loan);
        }

        public async Task<List<AccountBase>> GetClientAccountsAsync(User executor, int clientId)
        {
            if (executor.Id != clientId && 
                !_authService.CheckPermission(executor, Permission.ViewAllLogs))
                throw new UnauthorizedAccessException();

            return await _accountRepo.GetAccountsByOwnerAsync(clientId);
        }

    }