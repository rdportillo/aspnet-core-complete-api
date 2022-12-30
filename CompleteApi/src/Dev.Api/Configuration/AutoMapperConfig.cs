using AutoMapper;
using Dev.Api.DTO;
using Dev.Business.Models;

namespace Dev.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
