
using AutoMapper;
using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ProductInterfaces;


namespace BlueBerry24.Application.Services.Concretes.ProductServiceConcretes
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<bool> AddProductCategoryAsync(ProductDto product, List<int> categories)
        {
            if(categories.Count == 0)
            {
                return false;
            }

            var mappedProduct = _mapper.Map<Product>(product);

            var created = await _productCategoryRepository.AddProductCategoryAsync(mappedProduct, categories);

            return created;
        }

        public async Task<bool> UpdateProductCategoryAsync(ProductDto product, List<int> categories)
        {
            if (categories.Count == 0 || !await _productRepository.ExistsByIdAsync(product.Id))
            {
                return false;
            }

            //await _unitOfWork.BeginTransactionAsync();

            var existingCategories = await _productCategoryRepository.GetCategoriesByProuductId(product.Id);
            
            if(!await _productCategoryRepository.RemoveCategoriesByProductId(product.Id))
            {
                //await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            //await _unitOfWork.CommitTransactionAsync();

            var mappedProduct = _mapper.Map<Product>(product);

            bool result = await _productCategoryRepository.AddProductCategoryAsync(mappedProduct, categories);

            return result;
        }
    }
}
