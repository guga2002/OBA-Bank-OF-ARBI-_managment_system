using Bank_Managment_System.Helper_Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{

    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public ValuteEnum Valute { get; set; }

        [Required]
        public string? Sender_Iban { get; set; }

        [Required]
        public string? Reciever_Iban { get; set; }

        [Required]
        public string Transaction_Type { get; set; }

        public decimal? TotalIncome { get; set; }

        [Required]
        public DateTime TransactTime { get; set; }

        //card relation 
        [ForeignKey("cards")]
        public int CardID { get; set; }
        public Card cards { get; set; }

    }


}
