using AspProject_Entities.ModelsValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspProject_Entities.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required field"), Column(TypeName = "smalldatetime"), BirthDateValidation(ErrorMessage = "1900 > Birthdate > Today")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Required field"), EmailAddress(ErrorMessage = "Illegal Email Address"), Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required field"), MinLength(8, ErrorMessage = "Min. of 8 characters")]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "שדה חובה")]
        [Compare("Password", ErrorMessage = "the passwords are not the same")]
        public string Confirmpassword { get; set; }
    }
}
