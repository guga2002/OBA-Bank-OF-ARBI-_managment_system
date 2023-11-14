using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Required]
        public string Tel { get; set; }
        public string? Address { get; set; }
         
        [Required]
        public string Gmail { get; set; }

        public List<Operator> operatori { get; set; }//1:1 kavshiri

        public List<User> user { get; set; } //1:1 kavshori

        public List<Manager> manager { get; set; } //1:1 kavshiri

    }
}
