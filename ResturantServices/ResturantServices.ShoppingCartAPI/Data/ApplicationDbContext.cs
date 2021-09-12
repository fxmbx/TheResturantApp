using Microsoft.EntityFrameworkCore;
using ResturantServices.ShoppingCartAPI.Models;
namespace ResturantServices.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext   
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
       {
           
       }
       public DbSet<Product> Products {get; set;}
       public DbSet<CartHeader> CartHeaders {get; set;}
       public DbSet<CartDetails> CartDetails {get; set;}
    }
}
