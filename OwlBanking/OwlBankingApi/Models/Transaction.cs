using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlBankingApi.Models;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TransactionId { get; set; }

    public decimal TransactionAmount    { get; set; }
    
    public DateTime TransactionDate { get; set; }
    
    
    [ForeignKey("BankAccountId")]
    public Guid SourceBankAccountId { get; set; }
    public virtual BankAccount? SourceBankAccount  { get; set; }
    
    [ForeignKey("BankAccountId")]
    public Guid DestinationBankAccountId { get; set; }
    public virtual BankAccount? DestinationBankAccount  { get; set; }
}