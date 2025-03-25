using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface IBankService
{
    void RegisterClient(Client client);
    
    void RegisterEnterprise(Enterprise enterprise);
}