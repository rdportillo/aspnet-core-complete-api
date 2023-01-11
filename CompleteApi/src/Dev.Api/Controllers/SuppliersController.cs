using AutoMapper;
using Dev.Api.Attributes;
using Dev.Api.DTO;
using Dev.Business.Interfaces;
using Dev.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;
        private readonly IAddressRepository _addressRepository;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService,
                                   IMapper mapper,
                                   INotifier notifier,
                                   IAddressRepository addressRepository,
                                   IUser user) : base(notifier, user)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
            _addressRepository = addressRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierDto>> GetAll()
        {
            var suppliers = _mapper.Map<IEnumerable<SupplierDto>>(await _supplierRepository.GetAll());

            return suppliers;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierDto>> GetById(Guid id)
        {
            var supplier = await GetSupplierProductsAddress(id);

            if (supplier == null) return NotFound();

            return supplier;
        }

        [ClaimsAuthorize("Suppliers", "Add")]
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> Add(SupplierDto supplierDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Add(_mapper.Map<Supplier>(supplierDto));

            return CustomResponse(supplierDto);
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierDto>> Update(Guid id, SupplierDto supplierDto)
        {
            if (id != supplierDto.Id)
            {
                NotifyError("The id is not the same one informed in the query");
                return CustomResponse(supplierDto);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Update(_mapper.Map<Supplier>(supplierDto));

            return CustomResponse(supplierDto);
        }

        [ClaimsAuthorize("Suppliers", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierDto>> Delete(Guid id)
        {
            var supplierDto = await GetSupplierAddress(id);

            if(supplierDto == null) return NotFound();

            await _supplierService.Remove(id);

            return CustomResponse(supplierDto);
        }

        [HttpGet("get-address/{id:guid}")]
        public async Task<AddressDto> GetAddressById(Guid id)
        {
            return _mapper.Map<AddressDto>(await _addressRepository.GetById(id));
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [HttpPut("update-address/{id:guid}")]
        public async Task<ActionResult> UpdateAddress(Guid id, AddressDto addressDto)
        {
            if (id != addressDto.Id)
            {
                NotifyError("The id is not the same one informed in the query");
                return CustomResponse(addressDto);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            await _supplierService.AddressUpdate(_mapper.Map<Address>(addressDto));

            return CustomResponse(addressDto);
        }

        private async Task<SupplierDto> GetSupplierProductsAddress(Guid id)
        {
            return _mapper.Map<SupplierDto>(await _supplierRepository.GetSupplierProductsAddress(id));
        }

        private async Task<SupplierDto> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierDto>(await _supplierRepository.GetSupplierAddress(id));
        }

        
    }
}
