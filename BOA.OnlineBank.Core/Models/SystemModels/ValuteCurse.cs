using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models.SystemModels
{
    [Table("ValuteCurses")]
    public class ValuteCurse
    {
        [Key]
        public int ValuteCursesID { get; set; }

        public decimal USD { get; set; }

        public decimal EURO { get; set; }

        public decimal GEL { get; set; }
    }
}
