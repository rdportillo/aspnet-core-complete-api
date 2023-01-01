using Dev.Business.Interfaces;
using Dev.Business.Models;
using Dev.Business.Models.Validations;

namespace Dev.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(ISupplierRepository supplierRepository, 
                               IAddressRepository addressRepository,
                               INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Supplier supplier)
        {
            if(!ValidationExecute(new SupplierValidation(), supplier)
                || !ValidationExecute(new AddressValidation(), supplier.Address)) return false;

            if(_supplierRepository.Search(s => s.Document == supplier.Document).Result.Any())
            {
                Notify("There is already a supplier with the same document");
                return false;
            }

            await _supplierRepository.Add(supplier);
            return true;
        }

        public async Task<bool> Update(Supplier supplier)
        {
            if (!ValidationExecute(new SupplierValidation(), supplier)) return false;

            if (_supplierRepository.Search(s => s.Document == supplier.Document && s.Id != supplier.Id).Result.Any())
            {
                Notify("There is already a supplier with the same informed document");
                return false;
            }

            await _supplierRepository.Update(supplier);
            return true;
        }

        public async Task AddressUpdate(Address address)
        {
            if (!ValidationExecute(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }

        public async Task<bool> Remove(Guid id)
        {
            if(_supplierRepository.GetSupplierProductsAddress(id).Result.Products.Any())
            {
                Notify("The supplier has products attached to it!");
                return false;
            }

            var address = await _addressRepository.GetAddressBySupplier(id);

            if(address != null)
            {
                await _addressRepository.DeleteById(address.Id);
            }

            await _supplierRepository.DeleteById(id);
            return true;
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
