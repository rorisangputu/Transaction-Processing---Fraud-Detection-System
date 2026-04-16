using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.Models;

/// <summary>
/// Represents a bank account owned by a user and involved in transactions.
/// </summary>
public class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public ICollection<Transaction> SourceTransactions { get; set; } = new List<Transaction>();

    public ICollection<Transaction> DestinationTransactions { get; set; } = new List<Transaction>();
}