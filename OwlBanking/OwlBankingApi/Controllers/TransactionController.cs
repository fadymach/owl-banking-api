using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        private readonly IBankAccountRepository _bankAccountRepository;
        public TransactionController(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
        {
            this._transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this._bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            Transaction? transaction = await this._transactionRepository.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/Transaction
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this._transactionRepository.AddTransactionAsync(transaction);
                    await this._transactionRepository.SaveAsync();
                    
                    BankAccount updatedDestinationBankAccount =
                        await this._bankAccountRepository.GetBankAccountByIdAsync(transaction.DestinationBankAccountId);
                    
                    return CreatedAtAction("PostTransaction", new { id = transaction.TransactionId }, transaction);
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            
            return BadRequest(); 
        }
    }
}
