using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantServices.ShoppingCartAPI.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]//makes the id the primary ky but manualy entered
        public int ProductId {get; set;}
        [Required]
        public string? Name {get; set;}
        [Range(1,10000)]
        public double Price {get; set;}
        
        public string? Description {get; set;}

        public string? CategoryName {get; set;}
        public string? ImageUrl{get;set;}
    }
}