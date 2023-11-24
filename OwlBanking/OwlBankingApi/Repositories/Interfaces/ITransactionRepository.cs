using OwlBankingApi.Models;

namespace OwlBankingApi.Repositories.Interfaces;

public interface ITransactionRepository : IOwlRepository, IDisposable 
{ 
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);
}