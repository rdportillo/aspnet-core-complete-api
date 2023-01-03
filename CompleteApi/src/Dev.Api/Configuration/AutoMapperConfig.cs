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
            CreateMap<ProductDto, Product>();
            CreateMap<ProductImageDto, Product>();
            CreateMap<Product, ProductImageDto>()
                .ForMember(dest => dest.SupplierName, options => options.MapFrom(src => src.Supplier.Name));
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.SupplierName, options => options.MapFrom(src => src.Supplier.Name));
        }
    }
}
