using AutoMapper;
using ResturantServices.ShoppingCartAPI.Models;
using ResturantServices.ShoppingCartAPI.Models.Dto;

namespace ResturantServices.ShoppingCartAPI
{
    public class AutoMapperProfile
    {
      public static MapperConfiguration RegisterMaps()
      {
          var mappingconfig = new MapperConfiguration(x => 
          {
             x.CreateMap<Product,ProductDto>().ReverseMap();
             x.CreateMap<CartHeader,CartHeaderDto>().ReverseMap();
             x.CreateMap<CartDetails,CartDetailsDto>().ReverseMap();
             x.CreateMap<Cart,CartDto>().ReverseMap();
          });
          return mappingconfig;
      }  
    }
}



// namespace ResturantServices.ShoppingCartAPI
// {
//     public class AutoMapperProfile 
//     {
//         // public static MapperConfiguration RegisterMaps()
//         // {
//         //     // var mappingconfig = new MapperConfiguration (x=> 
//         //     // {
//         //     //     x.CreateMap<Product,ProductDto>().ReverseMap();
//         //     // });
//         //     // return mappingconfig;
//         // }
//     }
// }