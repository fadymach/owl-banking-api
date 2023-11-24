using Microsoft.EntityFrameworkCore;
using OwlBankingApi.Database;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Repositories.Implementations;

public class BankAccountRepository : IBankAccountRepository
{
    private OwlBankingDbContext _dbContext;

    public BankAccountRepository(OwlBankingDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync()
    {
        return await this._dbContext.BankAccount.ToListAsync();
    }
    
    public async Task<BankAccount?> GetBankAccountByIdAsync(Guid bankAccountId)
    {
        BankAccount bankAccount = await this._dbContext.BankAccount
            .Include(x => x.SourceTransactions)
            .Include(x => x.DestinationTransactions)
            .FirstOrDefaultAsync(x => x.BankAccountId == bankAccountId);
        if (bankAccount == null)
        {
            return null;
        }
        
        return bankAccount;
    }

    public async Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount)
    {
        await this._dbContext.BankAccount.AddAsync(bankAccount);
        return bankAccount;
    }

    public async Task<decimal?> GetBankAccountBalanceAsync(Guid bankAccountId)
    {
        BankAccount bankAccount = await this._dbContext.BankAccount.FirstOrDefaultAsync(x => x.BankAccountId == bankAccountId);
        if (bankAccount == null)
        {
            return null;
        }

        return bankAccount.Balance;
    }

    public async Task<List<Transaction?>> GetBankAccountTransactionsAsync(Guid bankAccountId)
    {
        ICollection<Transaction?> sourceTransactions = await GetBankAccountSourceTransactionsAsync(bankAccountId);
        ICollection<Transaction?> destinationTransactions = await GetBankAccountDestinationTransactionsAsync(bankAccountId);
        
        return destinationTransactions.Concat(sourceTransactions).ToList();
    }
    
    public async Task<ICollection<Transaction?>> GetBankAccountSourceTransactionsAsync(Guid bankAccountId)
    {
        ICollection<Transaction?> sourceTransactions = await this._dbContext.Transaction.Where(x => x.SourceBankAccountId == bankAccountId).ToListAsync();
        return sourceTransactions;
    }
    
    public async Task<ICollection<Transaction?>> GetBankAccountDestinationTransactionsAsync(Guid bankAccountId)
    {
        ICollection<Transaction?> destinationTransactions = await this._dbContext.Transaction.Where(x => x.DestinationBankAccountId == bankAccountId).ToListAsync();
        return destinationTransactions;
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