using Bank_Managment_System.Models.Aunthification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Operators")]
    public class Operator
    {
        [Key]
        public int OperatorID { get; set; }

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
        [MaxLength(50)]
        public string Position { get; set; }

        public DateTime RegistrationDate { get; set; }

        [ForeignKey("contact")]
        public int ContactID { get; set; }

        public Contact contact { get; set; }

        public CookieforOperator cookie { get; set; }

        public OperatorAunthification aunthification { get; set; }

        [Required]
        public int ManagerID { get; set; }

    }
}
