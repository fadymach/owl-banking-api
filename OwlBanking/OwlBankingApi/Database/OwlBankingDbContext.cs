using Microsoft.EntityFrameworkCore;
using OwlBankingApi.Models;

namespace OwlBankingApi.Database;

public class OwlBankingDbContext : DbContext
{
    public OwlBankingDbContext(DbContextOptions<OwlBankingDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<BankAccount>? BankAccount { get; set; }
    public DbSet<Transaction?>? Transaction { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                PersonId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa1"),
                FirstName = "Arisha",
                MiddleName = "Carron",
                LastName = "Barron"
            },
            new Person
            {
                PersonId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                FirstName = "Branden",
                LastName = "Gibson"
            },
            new Person
            {
                PersonId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa3"),
                FirstName = "Rhonda",
                MiddleName = "Praya",
                LastName = "Church"
            },
            new Person
            {
                PersonId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa4"),
                FirstName = "Georgina",
                LastName = "Hazel"
            }
        );

        modelBuilder.Entity<BankAccount>()
            .HasKey(ba => new {ba.BankAccountId, ba.PersonId});

        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.Person)
            .WithMany(p => p.Accounts)
            .HasForeignKey(ba => ba.PersonId);
        
        
        modelBuilder.Entity<Transaction>()
            .HasKey(t => new {t.TransactionId, t.DestinationBankAccountId});
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.DestinationBankAccount)
            .WithMany(ba => ba.DestinationTransactions)
            .HasForeignKey(t => t.DestinationBankAccountId)
            .HasPrincipalKey(ba => ba.BankAccountId);

        modelBuilder.Entity<Transaction>()
            .HasKey(t => new {t.TransactionId, t.SourceBankAccountId});
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SourceBankAccount)
            .WithMany(ba => ba.SourceTransactions)
            .HasForeignKey(t => t.SourceBankAccountId)
            .HasPrincipalKey(ba => ba.BankAccountId);
    }
}