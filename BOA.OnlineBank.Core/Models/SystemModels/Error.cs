using ErrorEnumi;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.SystemModels
{
    [Table("Errors")]
    public class Error
    {
        [Key]
        public int ErrorId { get; set; }

        [MaxLength(200)]
        [Required]
        public String Description { get; set; }

        [Required]
        public DateTime TimeOfocured { get; set; }

        public ErrorEnum errortype { get; set; }


    }
}
