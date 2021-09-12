using System.Collections.Generic;
namespace ResturantServices.ShoppingCartAPI.Models
{
    public class Cart
    {
        public CartHeader CartHeader {get; set;}

        public IEnumerable<CartDetails> CartDetails {get; set;}
    }
}