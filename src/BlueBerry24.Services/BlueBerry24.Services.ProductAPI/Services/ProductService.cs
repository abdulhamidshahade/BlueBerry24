using AutoMapper;
using BlueBerry24.Services.ProductAPI.Services.Interfaces;
using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Services.Generic;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
using BlueBerry24.Services.ProductAPI.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;
using BlueBerry24.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductCategoryService _productCategoryService;
        private readonly ApplicationDbContext _context;

        public ProductService(IRepository<Product> productRepository, IUnitOfWork unitOfWork, 
            IMapper mapper,
            IProductCategoryService productCategoryService,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productCategoryService = productCategoryService;
            _context = context;
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            var product = await _context.Products.Where(i => i.Id == id)
                .Include(c => c.ProductCategories)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new NotFoundException($"Product with id: {id} not found");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Product with name: {name} cannot be empty", nameof(name));
            }

            var product = await _context.Products.Where(n => n.Name == name)
                .Include(c => c.ProductCategories)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync();
            
            if (product == null)
            {
                throw new NotFoundException($"Product with name: {name} not found");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.Include(c => c.ProductCategories).ThenInclude(c => c.Category).ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto productDto, List<string> categories)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            if (await ExistsByNameAsync(productDto.Name))
            {
                throw new DuplicateEntityException($"Product with name {productDto.Name} already exists");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var product = _mapper.Map<Product>(productDto);

                await _productRepository.AddAsync(product);

                await _unitOfWork.SaveChangesAsync();

                if(!await _productCategoryService.AddProductCategoryAsync(product, categories))
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to add product categories.");
                }

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                product = await _context.Products.Where(i => i.Id == product.Id).Include(c => c.ProductCategories)
                    .ThenInclude(c => c.Category).FirstOrDefaultAsync();

                return _mapper.Map<ProductDto>(product);
            }
        }


        public async Task<ProductDto> UpdateAsync(string id, UpdateProductDto productDto, List<string> categories)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product with id: {id} not found");
            }

            var ProductWithSameName = await _productRepository.GetAsync(c => c.Name == productDto.Name && c.Id != id);

            if (ProductWithSameName != null)
            {
                throw new DuplicateEntityException($"Product with name: {productDto.Name} already exists");
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.StockQuantity = productDto.StockQuantity;
            existingProduct.ImageUrl = productDto.ImageUrl;

            _productRepository.Update(existingProduct);

            await _productCategoryService.UpdateProductCategoryAsync(existingProduct, categories);

            await _unitOfWork.SaveChangesAsync();

            existingProduct = await _context.Products.Where(i => i.Id == existingProduct.Id)
                .Include(c => c.ProductCategories)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync();

            return _mapper.Map<ProductDto>(existingProduct);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await _productRepository.ExistsAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty", nameof(name));
            }

            return await _productRepository.ExistsAsync(c => c.Name == name);
        }



        public async Task<bool> ExistsByShopIdAsync(string productId, string shopId)
        {
            var exists = await _context.Products.Where(i => i.Id == productId && i.ShopId == shopId).AnyAsync();

            return exists;
        }
    }
}
