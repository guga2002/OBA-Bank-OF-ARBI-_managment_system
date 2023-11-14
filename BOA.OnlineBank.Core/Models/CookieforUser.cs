using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("CookieforUsers")]
    public class CookieforUser
    {
        [Key]
        public int cookieID { get; set; }

        public string token { get; set; }

        [ForeignKey("users")]
        public int UserID { get; set; }

        public User users { get; set; }
    }
}
