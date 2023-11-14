using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.Aunthification
{
    [Table("OperatorAunthifications")]
    public class OperatorAunthification
    {
        [Key]
        public int OperatorAunthificationID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [ForeignKey("operatori")]
        public int OperatorID { get; set; }

        public Operator operatori { get; set; }
    }
}
