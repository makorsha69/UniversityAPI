using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models
{
    public class University
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ID will be automatically generated")]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string? country { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string? alpha_two_code { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string? name { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string state_province { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string? domain { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string? web_pages { get; set; }


    }
}
