using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Cookieformanagers")]
    public class Cookieformanager
    {
        [Key]
        public int cookieID { get; set; }

        [Required]
        public string token { get; set; }

        [ForeignKey("managers")]
        public int managerID { get; set; }

        public Manager managers { get; set; }
    }
}
