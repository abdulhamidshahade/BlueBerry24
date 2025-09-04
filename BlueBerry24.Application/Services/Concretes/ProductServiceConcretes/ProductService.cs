
using AutoMapper;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ProductInterfaces;

namespace BlueBerry24.Application.Services.Concretes.ProductServiceConcretes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IUnitOfWork _unitOfWork;


        public ProductService(IProductRepository productRepository,
                              IMapper mapper,
                              IProductCategoryService productCategoryService,
                              IUnitOfWork unitOfWork
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productCategoryService = productCategoryService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var product = await _productRepository.GetByNameAsync(name);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(product);
        }



        public async Task<ProductDto> CreateAsync(CreateProductDto productDto, List<int> categories)
        {
            if (productDto == null)
            {
                return null;
            }

            if (await ExistsByNameAsync(productDto.Name))
            {
                return null;
            }

            await _unitOfWork.BeginTransactionAsync();

            var product = _mapper.Map<Product>(productDto);
            var createdProduct = await _productRepository.CreateAsync(product);

            var productToDto = _mapper.Map<ProductDto>(createdProduct);

            if (!await _productCategoryService.AddProductCategoryAsync(productToDto, categories))
            {
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<ProductDto>(await _productRepository.GetByIdAsync(createdProduct.Id));
        }


        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto productDto, List<int> categories)
        {
            if (productDto == null)
            {
                return null;
            }

            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return null;
            }

            var isProductExists = await GetByNameAsync(productDto.Name);

            if (isProductExists != null && isProductExists.Id != id)
            {
                return null;
            }

            var mappedProduct = _mapper.Map<Product>(productDto);

            await _unitOfWork.BeginTransactionAsync();

            var updatedProduct = await _productRepository.UpdateAsync(id, mappedProduct);

            var productToDto = _mapper.Map<ProductDto>(updatedProduct);

            if (!await _productCategoryService.UpdateProductCategoryAsync(productToDto, categories))
            {
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }

            await _unitOfWork.CommitTransactionAsync();

            var returnProduct = await _productRepository.GetByIdAsync(updatedProduct.Id);

            return _mapper.Map<ProductDto>(returnProduct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            return await _productRepository.DeleteAsync(product);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _productRepository.ExistsByIdAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return await _productRepository.ExistsByNameAsync(name);
        }

        public async Task<PaginationDto<ProductDto>> GetPaginatedAsync(ProductFilterDto filter)
        {

            var products = await _productRepository.GetFilteredAsync(
                filter.SearchTerm,
                filter.Category,
                filter.SortBy,
                filter.MinPrice,
                filter.MaxPrice,
                filter.IsActive,
                filter.PageNumber,
                filter.PageSize
            );

            var totalCount = await _productRepository.GetFilteredCountAsync(
                filter.SearchTerm,
                filter.Category,
                filter.MinPrice,
                filter.MaxPrice,
                filter.IsActive
            );

            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            var paginationResult = new PaginationDto<ProductDto>(
                productDtos,
                filter.PageNumber,
                filter.PageSize,
                totalCount
            );

            return paginationResult;
        }
    }



    //public async Task<bool> ExistsByShopIdAsync(string productId, string shopId)
    //{
    //    var exists = await _context.Products.Where(i => i.Id == productId && i.ShopId == shopId).AnyAsync();

    //    return exists;
    //}
}
