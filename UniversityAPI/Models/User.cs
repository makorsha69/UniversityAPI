using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ID will be automatically generated")]
        public int UserId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "The train name cannot be empty.")]
        public string? Username { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "The email cannot be empty.")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(25)")]
        [Required(ErrorMessage = "Password is Compulsory.")]
        public string? Password { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? Role { get; set; }

        public Boolean Access { get; set; }

        public string? Status { get; set; }

    }
}
