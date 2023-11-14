using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.Aunthification
{
    [Table("ManagerAuthifications")]
    public class ManagerAuthification
    {
        [Key]
        public int ManagerAuthificationID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [ForeignKey("manager")]
        public int ManagerID { get; set; }

        public Manager manager { get; set; }
    }
}
