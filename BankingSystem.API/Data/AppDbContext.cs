using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Data;

/// <summary>
/// Represents the application's database context and configures entity relationships.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<FraudFlag> FraudFlags => Set<FraudFlag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.HasMany(u => u.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasIndex(a => a.AccountNumber)
                .IsUnique();

            entity.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(a => a.Balance)
                .HasPrecision(18, 2);

            entity.Property(a => a.CreatedAt)
                .IsRequired();

            entity.HasMany(a => a.SourceTransactions)
                .WithOne(t => t.SourceAccount)
                .HasForeignKey(t => t.SourceAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(a => a.DestinationTransactions)
                .WithOne(t => t.DestinationAccount)
                .HasForeignKey(t => t.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.Amount)
                .HasPrecision(18, 2);

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.HasMany(t => t.FraudFlags)
                .WithOne(f => f.Transaction)
                .HasForeignKey(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FraudFlag>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Reason)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(f => f.CreatedAt)
                .IsRequired();
        });
    }
}
