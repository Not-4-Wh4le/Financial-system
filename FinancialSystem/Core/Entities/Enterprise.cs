﻿using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Entities;

public class Enterprise : IBankClient
{
    
    public int Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public EnterpriseType Type { get; set; }
    public string UNP { get; set; } = string.Empty;
    public string BIC { get; set; } = string.Empty;
    public Bank Bank { get; set; } = null!;
    public List<AccountBase> Accounts { get; set; } = new();

    public string Name
    {
        get => this.LegalName;
        set => this.LegalName = value;
    }

}