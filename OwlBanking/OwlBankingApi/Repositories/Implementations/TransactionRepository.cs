using Microsoft.EntityFrameworkCore;
using OwlBankingApi.Database;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Repositories.Implementations;

public class TransactionRepository : ITransactionRepository
{
    private OwlBankingDbContext _dbContext;

    public TransactionRepository(OwlBankingDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        await this._dbContext.Transaction.AddAsync(transaction);
        
        Guid sourceBankAccountId = transaction.SourceBankAccountId;
        BankAccount? sourceBankAccount = await this._dbContext.BankAccount.FirstOrDefaultAsync(x => x.BankAccountId == sourceBankAccountId);
        Guid destinationBankAccountId = transaction.DestinationBankAccountId;
        BankAccount? destinationBankAccount = await this._dbContext.BankAccount.FirstOrDefaultAsync(x => x.BankAccountId == destinationBankAccountId);

        if (sourceBankAccount == null || destinationBankAccount == null)
        {
            return null;
        }
        
        // For brevity I am not doing any balance validation for the transfers to occur. I am assuming the source person has monies.  
        sourceBankAccount.Balance -= transaction.TransactionAmount;
        destinationBankAccount.Balance += transaction.TransactionAmount;

        return transaction;
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
    {
        Transaction? transaction = await this._dbContext.Transaction.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        return transaction ?? null;
    }

    public Task SaveAsync()
    {
        try
        {
            this._dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);

        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}