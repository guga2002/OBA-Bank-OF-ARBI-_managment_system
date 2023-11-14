using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Managment_System.Models._2Fa
{
    [Table("2stepCodes")]
    public class _2StepAunt
    {
        [Key]
        public  int stepID { get; set; }

        [Required]
        public string code { get; set; }

        public int OwnerID { get; set; }// ar vaqoneqtebt tablbetan radgan gvyavs bevri momxarebeli da jobia iyos  marto veli 
    }
}
