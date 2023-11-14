using Bank_Managment_System.Helper_Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("BankAccounts")]
    public class BankAccount
    {
        [Key]
        public int BankAccountID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Iban { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public ValuteEnum Valute { get; set; }

        public decimal AmountWithdrawed { get; set; }

        public DateTime LastWithdrawDate { get; set; }

        public decimal totalwithdrawed { get; set; }

        [ForeignKey("user")]
        public int UserID { get; set; }

        public User user { get; set; }

        public List<Card> card { get; set; }
    }
}
