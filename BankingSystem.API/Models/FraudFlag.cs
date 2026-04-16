using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.Models;

/// <summary>
/// Represents a fraud detection alert linked to a specific transaction.
/// </summary>
public class FraudFlag
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Transaction Transaction { get; set; } = null!;
}
