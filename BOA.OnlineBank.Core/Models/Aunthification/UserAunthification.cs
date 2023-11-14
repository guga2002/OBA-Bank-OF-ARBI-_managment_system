using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.Aunthification
{
    [Table("UserAunthifications")]
    public class UserAunthification
    {
        [Key]
        public int UserAunthificationID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public  string Password { get; set; }

        [ForeignKey("user")]
        public int UserID { get; set; }

        public  User user { get; set; }

    }
}
