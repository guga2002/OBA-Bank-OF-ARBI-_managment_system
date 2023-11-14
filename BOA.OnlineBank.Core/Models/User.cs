using Bank_Managment_System.Models.Aunthification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(11)]
        public string PN { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public DateTime Registration_date { get; set; } = DateTime.Now;
        public String? Status { get; set; }

        [ForeignKey("contact")]
        public int ContactID { get; set; }

        public Contact contact { get; set; }

        public List<BankAccount> accounts { get; set; }

        public CookieforUser cook { get; set; }

        public UserAunthification aunthification { get; set; }

        public int  OperatorID { get; set; }
    }
}
