using Dev.Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dev.Api.DTO
{
    public class SupplierDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(100, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(14, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }

        public int SupplierType { get; set; }

        public AddressDto Address { get; set; }

        public bool Active { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
    }
}
