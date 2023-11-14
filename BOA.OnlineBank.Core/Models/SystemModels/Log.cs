using ErrorEnumi;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.SystemModels
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Acction { get; set; }

        [Required]
        public DateTime timeofocured { get; set; }

        public ErrorEnum type { get; set; }
    }
}
