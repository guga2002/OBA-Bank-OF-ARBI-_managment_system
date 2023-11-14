using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Cards")]
    public class Card
    {
        [Key]
        public int cardId { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public DateTime Validatedate { get; set; }
        [Required]
        [MaxLength(3)]
        public string CVVCode { get; set; }

        [Required]
        [MaxLength(4)]
        public string PINcode { get; set; }

        public string? AuthorizedStatus { get; set; }


        [ForeignKey("account")]
        public int  accountId { get; set; }

        public BankAccount account { get; set; }

        public List<Transaction> transacts { get; set; }


    }
}
