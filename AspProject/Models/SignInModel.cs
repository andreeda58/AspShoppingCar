using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspProject_Entities.Models
{
    
    public class SignInModel
    {
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required field"), MinLength(8, ErrorMessage = "Min. of 8 characters")]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }

    }
}
