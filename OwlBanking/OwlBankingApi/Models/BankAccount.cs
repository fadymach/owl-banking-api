using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlBankingApi.Models;

public class BankAccount
{
    // Unique Identifier for the Bank Account
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BankAccountId { get; set; }

    public int AccountNumber { get; set; } = 123451234;
    
    // This will return to use ta formatted style of the account number as I have usually seen it this way. 
    public string AccountNumberFormatted
    {
        // I'm making assumptions account the format/length of a bank account number.
        get { return String.Format("{0:00000-0000}", this.AccountNumber); }
    }

    public decimal Balance { get; set; } = 0;
    
    public AccountType AccountType { get; set; }

    // When the Person initially created this specific bank account associated with their account
    public DateTime SignUpDate { get; set; }
    
    [ForeignKey("PersonId")]
    public Guid PersonId { get; set; }
    public virtual Person? Person { get; set; }

    public ICollection<Transaction> DestinationTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> SourceTransactions { get; set; } = new List<Transaction>();
}