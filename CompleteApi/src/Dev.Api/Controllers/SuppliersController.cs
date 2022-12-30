using AutoMapper;
using Dev.Api.DTO;
using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.Controllers
{
    [Route("api/[controller]")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDto>> GetAll()
        {
            var suppliers = _mapper.Map<IEnumerable<SupplierDto>>(await _supplierRepository.GetAll());

            return suppliers;
        }
    }
}
