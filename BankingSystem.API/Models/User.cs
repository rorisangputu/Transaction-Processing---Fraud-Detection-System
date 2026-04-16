using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.Models;

/// <summary>
/// Represents a banking system user who can own one or more accounts.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
