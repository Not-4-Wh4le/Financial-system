using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Entities;

    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bic { get; set; } = string.Empty;
        public List<User> ClientUsers { get; set; } = new();
    
        public List<Enterprise> ClientEnterprises { get; set; } = new();
        
        public List<IBankClient> Clients => ClientUsers.Cast<IBankClient>().Concat(ClientEnterprises).ToList();
    }