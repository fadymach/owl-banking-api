using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountController(IBankAccountRepository bankAccountRepository)
        {
            this._bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        // GET: api/BankAccount/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BankAccount>> GetBankAccount(Guid id)
        { 
            BankAccount? bankAccount = await this._bankAccountRepository.GetBankAccountByIdAsync(id);
            
            if (bankAccount == null) 
            { 
                return NotFound(); 
            }
              
            return Ok(bankAccount);
        }
        
        // GET: api/BankAccountTransactions/5
        [HttpGet("BankAccountTransactions/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBankAccountTransactions(Guid id)
        {
            List<Transaction?> transactions = await this._bankAccountRepository.GetBankAccountTransactionsAsync(id);

            return Ok(transactions);
        }
        
        // GET: api/BankAccountBalance/5
        [HttpGet("BankAccountBalance/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<decimal>> GetBankAccountBalance(Guid id)
        {
            decimal? balance = await this._bankAccountRepository.GetBankAccountBalanceAsync(id);
          
            if (balance == null) 
            { 
                return NotFound(); 
            }
          
            return Ok(balance);
        }

        // POST: api/BankAccount
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this._bankAccountRepository.AddBankAccountAsync(bankAccount);
                    await this._bankAccountRepository.SaveAsync();
                    return CreatedAtAction("PostBankAccount", new { id = bankAccount.BankAccountId }, bankAccount);
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
