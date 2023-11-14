using Bank_Managment_System.Models.Aunthification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models
{
    [Table("Managers")]
    public class Manager
    {
        [Key]
        public int managerID { get; set; }
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

        public Cookieformanager cookie { get; set; }

        public ManagerAuthification authification { get; set; }

       // public List<Operator> operators { get; set; }
    }
}
