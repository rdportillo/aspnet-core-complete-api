using AutoMapper;
using Dev.Api.DTO;
using Dev.Business.Interfaces;
using Dev.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Dev.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  IProductService productService,
                                  IMapper mapper,
                                  INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetProductsSuppliers());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var productDto = await GetProduct(id);

            if (productDto == null) return NotFound();

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Add(ProductDto productDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productDto.Image;

            if (!FileUpload(productDto.ImageUpload, imageName)) return CustomResponse(productDto);

            productDto.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productDto));

            return CustomResponse(productDto);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ProductDto>> AddAlternative(ProductImageDto productImageDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagePrefix = Guid.NewGuid() + "_";

            if (!await FileUploadAlternative(productImageDto.ImageUpload, imagePrefix))
            {
                return CustomResponse(productImageDto);
            }

            productImageDto.Image = imagePrefix + productImageDto.ImageUpload.FileName;
            await _productService.Add(_mapper.Map<Product>(productImageDto));

            return CustomResponse(productImageDto);
        }

        [RequestSizeLimit(1000000)]
        [HttpPost("image")]
        public async Task<ActionResult<ProductDto>> AddImage(IFormFile image)
        {
            return Ok(image);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, ProductDto productDto)
        {
            if (id != productDto.Id) {
                NotifyError("The informed Ids are not the same!");
                return CustomResponse();
            }

            var productUpdateDto = await GetProduct(id);
            productDto.Image = productUpdateDto.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if(productDto.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productDto.Image;

                if(!FileUpload(productDto.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                productUpdateDto.Image = imageName;
            }

            productUpdateDto.Name = productDto.Name;
            productUpdateDto.Description = productDto.Description;
            productUpdateDto.Price = productDto.Price;
            productUpdateDto.Active = productDto.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdateDto));

            return CustomResponse(productDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductDto>> Delete(Guid id)
        {
            var productDto = await GetProduct(id);

            if (productDto == null) return NotFound();

            await _productService.Remove(id);

            return CustomResponse(productDto);
        }


        private async Task<ProductDto> GetProduct(Guid id)
        {
            return _mapper.Map<ProductDto>(await _productRepository.GetProductSupplier(id));
        }

        private bool FileUpload(string file, string imageName)
        {
            if(file == null || file.Length <=0)
            {
                NotifyError("Provide an image to this product!");
                return false;
            }

            var imageDateByteArray = Convert.FromBase64String(file);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product_images", imageName);

            if(System.IO.File.Exists(filePath))
            {
                NotifyError("There is already a file with the same name!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDateByteArray);
            return true;
        }

        private async Task<bool> FileUploadAlternative(IFormFile file, string imagePrefix)
        {
            if (file == null || file.Length == 0)
            {
                NotifyError("Provide an image to this product!");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product_images", imagePrefix + file.FileName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("There is already a file with the same name!");
                return false;
            }

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return true;
        }
    }
}
