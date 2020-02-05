using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models
{
    public class Email
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(10)")]
        public string EmailAddress { get; set; }
    }
}