using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("CookiesForOperators")]
    public class CookieforOperator
    {
        [Key]
        public int CookieID { get; set; }

        [ForeignKey("operatori")]
        public int OperatorID { get; set; }

        public Operator operatori { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
