using AutoMapper;
using ResturantServices.ProductApi.Models;
using ResturantServices.ProductApi.Models.Dto;

namespace ResturantServices.ProductApi
{
    public class AutoMapperProfile 
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration (x=> 
            {
                x.CreateMap<Product,ProductDto>().ReverseMap();
            });
            return mappingconfig;
        }
    }
}