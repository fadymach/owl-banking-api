using OwlBankingApi.Models;

namespace OwlBankingApi.Repositories.Interfaces;

public interface IBankAccountRepository : IOwlRepository, IDisposable 
{
        Task<BankAccount?> GetBankAccountByIdAsync(Guid bankAccountId);
        Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount);
        Task<decimal?> GetBankAccountBalanceAsync(Guid bankAccountId);
        Task<List<Transaction?>> GetBankAccountTransactionsAsync(Guid bankAccountId);
        Task<ICollection<Transaction?>> GetBankAccountSourceTransactionsAsync(Guid bankAccountId);
        Task<ICollection<Transaction?>> GetBankAccountDestinationTransactionsAsync(Guid bankAccountId);
}