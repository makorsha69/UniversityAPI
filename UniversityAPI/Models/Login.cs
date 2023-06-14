using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityAPI.Models
{
    public class Login
    {
        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "The email cannot be empty.")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "The password cannot be empty.")]
        public string? Password { get; set; }
    }
}
