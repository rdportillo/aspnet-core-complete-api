using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface ISupplierService : IDisposable
    {
        Task<bool> Add(Supplier supplier);

        Task<bool> Update(Supplier supplier);

        Task<bool> Remove(Guid id);

        Task AddressUpdate(Address address);
    }
}
