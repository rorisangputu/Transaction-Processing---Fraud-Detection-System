using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.Models;

/// <summary>
/// Represents a financial transaction such as a deposit, withdrawal, or transfer.
/// </summary>
public class Transaction
{
    public Guid Id { get; set; }

    public TransactionType Type { get; set; }

    public decimal Amount { get; set; }

    public Guid? SourceAccountId { get; set; }

    public Guid? DestinationAccountId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Account? SourceAccount { get; set; }

    public Account? DestinationAccount { get; set; }

    public ICollection<FraudFlag> FraudFlags { get; set; } = new List<FraudFlag>();
}
