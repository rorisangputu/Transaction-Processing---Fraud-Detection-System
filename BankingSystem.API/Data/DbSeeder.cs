using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Data
{
    /// <summary>
    /// Seeds realistic demo banking data for local development and dashboard testing.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            var now = DateTime.UtcNow;

            var users = new List<User>
        {
            CreateUser("Ava Johnson", "ava.johnson@example.com", now.AddMonths(-10)),
            CreateUser("Liam Smith", "liam.smith@example.com", now.AddMonths(-9)),
            CreateUser("Mia Williams", "mia.williams@example.com", now.AddMonths(-8)),
            CreateUser("Noah Brown", "noah.brown@example.com", now.AddMonths(-7)),
            CreateUser("Emma Jones", "emma.jones@example.com", now.AddMonths(-6)),
            CreateUser("Oliver Garcia", "oliver.garcia@example.com", now.AddMonths(-5)),
            CreateUser("Sophia Miller", "sophia.miller@example.com", now.AddMonths(-4)),
            CreateUser("Elijah Davis", "elijah.davis@example.com", now.AddMonths(-3)),
            CreateUser("Isabella Wilson", "isabella.wilson@example.com", now.AddMonths(-2)),
            CreateUser("Lucas Martinez", "lucas.martinez@example.com", now.AddMonths(-1))
        };

            var accounts = new List<Account>();
            var accountNumber = 100000001;

            foreach (var user in users)
            {
                accounts.Add(CreateAccount(user.Id, accountNumber++.ToString(), 50000m, 250000m, now.AddMonths(-8)));
                accounts.Add(CreateAccount(user.Id, accountNumber++.ToString(), 10000m, 150000m, now.AddMonths(-6)));
            }

            var transactions = new List<Transaction>
        {
            CreateTransaction(TransactionType.Deposit, 45000m, null, accounts[0].Id, now.AddMonths(-6).AddDays(2)),
            CreateTransaction(TransactionType.Deposit, 78000m, null, accounts[1].Id, now.AddMonths(-6).AddDays(8)),
            CreateTransaction(TransactionType.Withdraw, 12000m, accounts[2].Id, null, now.AddMonths(-5).AddDays(4)),
            CreateTransaction(TransactionType.Transfer, 30000m, accounts[0].Id, accounts[3].Id, now.AddMonths(-5).AddDays(9)),
            CreateTransaction(TransactionType.Transfer, 125000m, accounts[4].Id, accounts[5].Id, now.AddMonths(-5).AddDays(18)),
            CreateTransaction(TransactionType.Deposit, 95000m, null, accounts[6].Id, now.AddMonths(-4).AddDays(3)),
            CreateTransaction(TransactionType.Withdraw, 15000m, accounts[7].Id, null, now.AddMonths(-4).AddDays(7)),
            CreateTransaction(TransactionType.Transfer, 220000m, accounts[8].Id, accounts[9].Id, now.AddMonths(-4).AddDays(12)),
            CreateTransaction(TransactionType.Transfer, 18000m, accounts[10].Id, accounts[11].Id, now.AddMonths(-4).AddDays(22)),
            CreateTransaction(TransactionType.Deposit, 51000m, null, accounts[12].Id, now.AddMonths(-3).AddDays(5)),
            CreateTransaction(TransactionType.Transfer, 135000m, accounts[13].Id, accounts[14].Id, now.AddMonths(-3).AddDays(10)),
            CreateTransaction(TransactionType.Withdraw, 7000m, accounts[15].Id, null, now.AddMonths(-3).AddDays(14)),
            CreateTransaction(TransactionType.Transfer, 42000m, accounts[16].Id, accounts[17].Id, now.AddMonths(-3).AddDays(18)),
            CreateTransaction(TransactionType.Deposit, 300000m, null, accounts[18].Id, now.AddMonths(-3).AddDays(25)),
            CreateTransaction(TransactionType.Transfer, 25000m, accounts[19].Id, accounts[0].Id, now.AddMonths(-2).AddDays(2)),
            CreateTransaction(TransactionType.Withdraw, 19000m, accounts[1].Id, null, now.AddMonths(-2).AddDays(4)),
            CreateTransaction(TransactionType.Transfer, 175000m, accounts[2].Id, accounts[4].Id, now.AddMonths(-2).AddDays(7)),
            CreateTransaction(TransactionType.Deposit, 66000m, null, accounts[3].Id, now.AddMonths(-2).AddDays(13)),
            CreateTransaction(TransactionType.Transfer, 28000m, accounts[5].Id, accounts[7].Id, now.AddMonths(-2).AddDays(17)),
            CreateTransaction(TransactionType.Withdraw, 9000m, accounts[9].Id, null, now.AddMonths(-2).AddDays(24)),
            CreateTransaction(TransactionType.Deposit, 112000m, null, accounts[11].Id, now.AddMonths(-1).AddDays(1)),
            CreateTransaction(TransactionType.Transfer, 76000m, accounts[12].Id, accounts[14].Id, now.AddMonths(-1).AddDays(5)),
            CreateTransaction(TransactionType.Withdraw, 24000m, accounts[13].Id, null, now.AddMonths(-1).AddDays(8)),
            CreateTransaction(TransactionType.Transfer, 410000m, accounts[15].Id, accounts[16].Id, now.AddMonths(-1).AddDays(12)),
            CreateTransaction(TransactionType.Deposit, 39000m, null, accounts[17].Id, now.AddMonths(-1).AddDays(18)),
            CreateTransaction(TransactionType.Transfer, 54000m, accounts[18].Id, accounts[19].Id, now.AddMonths(-1).AddDays(22)),
            CreateTransaction(TransactionType.Withdraw, 13000m, accounts[0].Id, null, now.AddDays(-20)),
            CreateTransaction(TransactionType.Transfer, 145000m, accounts[1].Id, accounts[2].Id, now.AddDays(-16)),
            CreateTransaction(TransactionType.Deposit, 87000m, null, accounts[4].Id, now.AddDays(-14)),
            CreateTransaction(TransactionType.Transfer, 23000m, accounts[6].Id, accounts[8].Id, now.AddDays(-10)),
            CreateTransaction(TransactionType.Withdraw, 36000m, accounts[10].Id, null, now.AddDays(-8)),
            CreateTransaction(TransactionType.Transfer, 199000m, accounts[11].Id, accounts[13].Id, now.AddDays(-6)),
            CreateTransaction(TransactionType.Deposit, 44000m, null, accounts[15].Id, now.AddDays(-4)),
            CreateTransaction(TransactionType.Transfer, 27000m, accounts[16].Id, accounts[18].Id, now.AddDays(-2)),
            CreateTransaction(TransactionType.Withdraw, 115000m, accounts[19].Id, null, now.AddDays(-1))
        };

            var fraudFlags = transactions
                .Where(transaction => transaction.Amount >= 100000m)
                .Select(transaction => new FraudFlag
                {
                    Id = Guid.NewGuid(),
                    TransactionId = transaction.Id,
                    Reason = "High-value transaction detected.",
                    CreatedAt = transaction.CreatedAt
                })
                .ToList();

            fraudFlags.AddRange(CreateRapidTransactionFlags(transactions));

            await context.Users.AddRangeAsync(users);
            await context.Accounts.AddRangeAsync(accounts);
            await context.Transactions.AddRangeAsync(transactions);
            await context.FraudFlags.AddRangeAsync(fraudFlags);

            await context.SaveChangesAsync();
        }

        private static User CreateUser(string name, string email, DateTime createdAt)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                CreatedAt = createdAt
            };
        }

        private static Account CreateAccount(Guid userId, string accountNumber, decimal minBalance, decimal maxBalance, DateTime createdAt)
        {
            return new Account
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountNumber = accountNumber,
                Balance = minBalance + (maxBalance - minBalance) / 2,
                CreatedAt = createdAt
            };
        }

        private static Transaction CreateTransaction(
            TransactionType type,
            decimal amount,
            Guid? sourceAccountId,
            Guid? destinationAccountId,
            DateTime createdAt)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Type = type,
                Amount = amount,
                SourceAccountId = sourceAccountId,
                DestinationAccountId = destinationAccountId,
                CreatedAt = createdAt
            };
        }

        private static IEnumerable<FraudFlag> CreateRapidTransactionFlags(IEnumerable<Transaction> transactions)
        {
            return transactions
                .GroupBy(transaction => transaction.SourceAccountId ?? transaction.DestinationAccountId)
                .Where(group => group.Key.HasValue)
                .SelectMany(group =>
                {
                    var orderedTransactions = group
                        .OrderBy(transaction => transaction.CreatedAt)
                        .ToList();

                    return orderedTransactions
                        .Where((transaction, index) =>
                            index >= 3 &&
                            transaction.CreatedAt - orderedTransactions[index - 3].CreatedAt <= TimeSpan.FromMinutes(5))
                        .Select(transaction => new FraudFlag
                        {
                            Id = Guid.NewGuid(),
                            TransactionId = transaction.Id,
                            Reason = "Multiple transactions detected within a short time window.",
                            CreatedAt = transaction.CreatedAt
                        });
                });
        }
    }
}