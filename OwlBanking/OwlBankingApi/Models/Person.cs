using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlBankingApi.Models;

public class Person
{
    // Unique Identifier for the Person
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PersonId { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required]
    public string LastName { get; set; }

    // When the Person initially created an account with the Owl Bank
    public DateTime SignUpDate { get; set; }
    
    public ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();
}