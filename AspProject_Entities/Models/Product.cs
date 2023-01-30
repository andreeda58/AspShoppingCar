using AspProject_Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspProject_Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")] 
        public string Title { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(500)")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(4000)")]
        public string LongDescription { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime LastModified { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1,double.MaxValue,ErrorMessage ="The number must be greater than 0")]
        public double Price { get; set; }
        [Column(TypeName = "image")]
        public byte[] Picture1 { get; set; }
        [Column(TypeName = "image")]
        public byte[] Picture2 { get; set; }
        [Column(TypeName = "image")]
        public byte[] Picture3 { get; set; }
        [Required(ErrorMessage = "Required field")]
        public ItemState State { get; set; }
        public User Seler { get; set; }
        public User Buyer { get; set; }
    }
}
