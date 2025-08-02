using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.InventoryEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.InventoryInterfaces;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Application.Services.Concretes.InventoryServiceConcretes
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IProductRepository productRepository,
                                IInventoryRepository inventoryRepository,
                                IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsInStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product != null && product.StockQuantity >= quantity;
        }

        public async Task<bool> AddStockAsync(int productId, int quantity, string notes, int? performedByUserId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            if (quantity <= 0)
            {
                return false;
            }

            product.StockQuantity += quantity;
            product.UpdatedAt = DateTime.UtcNow;

            var updatedProduct = await _productRepository.UpdateAsync(productId, product);


            var inventoryLog = new InventoryLog
            {

                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = quantity,
                ChangeType = InventoryChangeType.Restock,
                Notes = notes,
                CreatedAt = DateTime.UtcNow,
                PerformedByUserId = performedByUserId
            };

            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

            if (createdInventory == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AdjustStockAsync(int productId, int newQuantity, string notes, int? performedByUserId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            if (newQuantity < 0)
            {
                return false;
            }

            int difference = newQuantity - product.StockQuantity;

            InventoryChangeType changeType = difference >= 0
                ? InventoryChangeType.StockAdjustment
                : InventoryChangeType.Restock;

            product.StockQuantity = newQuantity;

            if (product.StockQuantity < product.ReservedStock)
            {
                product.ReservedStock = product.StockQuantity;
            }

            product.UpdatedAt = DateTime.UtcNow;

            var inventoryLog = new InventoryLog
            {
                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = difference,
                ChangeType = changeType,
                Notes = notes,
                CreatedAt = DateTime.UtcNow,
                PerformedByUserId = performedByUserId
            };

            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

            if (createdInventory == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ConfirmStockDeductionAsync(int productId, int quantity, int referenceId, string referenceType)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            if (product.ReservedStock < quantity)
            {
                return false;
            }

            var strategy = _unitOfWork.BeginTransactionAsyncStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    product.ReservedStock -= quantity;
                    product.StockQuantity -= quantity;
                    product.UpdatedAt = DateTime.UtcNow;

                    var inventoryLog = new InventoryLog
                    {
                        ProductId = productId,
                        CurrentStockQuantity = product.StockQuantity,
                        QuantityChanged = -quantity,
                        ChangeType = InventoryChangeType.Purchase,
                        ReferenceId = referenceId,
                        ReferenceType = referenceType,
                        Notes = $"Purchased {quantity} units via {referenceType} {referenceId}",
                        CreatedAt = DateTime.UtcNow
                    };

                    var updatedProduct = await _productRepository.UpdateAsync(product.Id, product);
                    var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

                    if (createdInventory == null || updatedProduct == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }

                    return await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }
            });
            
        }


        public async Task<List<InventoryLog>> GetInventoryHistoryAsync(int productId, int limit = 50)
        {
            var inventoryHistory = await _inventoryRepository.GetInventoryHistoryAsync(productId, limit);

            return inventoryHistory;
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int limit = 50)
        {
            return await _inventoryRepository.GetLowStockProductsAsync(limit);
        }

        public async Task<Product> GetProductWithStockInfoAsync(int productId)
        {
            return await _inventoryRepository.GetProductWithStockInfoAsync(productId);
        }

        public async Task ProcessStockNotificationsAsync()
        {
            var lowStockProducts = await GetLowStockProductsAsync(100);

            foreach (var product in lowStockProducts)
            {
                var inventoryLog = new InventoryLog
                {
                    ProductId = product.Id,
                    CurrentStockQuantity = product.StockQuantity,
                    QuantityChanged = 0,
                    ChangeType = InventoryChangeType.StockAdjustment,
                    Notes = $"Low stock notification: {product.Name} has {product.StockQuantity} units (threshold: {product.LowStockThreshold})",
                    CreatedAt = DateTime.UtcNow
                };

                await _inventoryRepository.CreateInventory(inventoryLog);
            }
        }

        public async Task<bool> ReleaseReservedStockAsync(int productId, int quantity, int referenceId, string referenceType)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            if (product.ReservedStock < quantity)
            {
                return false;
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                product.ReservedStock -= quantity;
                product.UpdatedAt = DateTime.UtcNow;

                var inventoryLog = new InventoryLog
                {
                    ProductId = productId,
                    CurrentStockQuantity = product.StockQuantity,
                    QuantityChanged = 0,
                    ChangeType = InventoryChangeType.ReleaseReservation,
                    ReferenceId = referenceId,
                    ReferenceType = referenceType,
                    Notes = $"Released {quantity} units from {referenceType} {referenceId}",
                    CreatedAt = DateTime.UtcNow
                };

                var updatedProduct = await _productRepository.UpdateAsync(product.Id, product);
                var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

                if (createdInventory == null || updatedProduct == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                return await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> ReserveStockAsync(int productId, int quantity, int referenceId, string referenceType)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            int availableStock = product.StockQuantity - product.ReservedStock;
            if (availableStock < quantity)
                return false;

            var strategy = _unitOfWork.BeginTransactionAsyncStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    product.ReservedStock += quantity;
                    product.UpdatedAt = DateTime.UtcNow;

                    var inventoryLog = new InventoryLog
                    {
                        ProductId = productId,
                        CurrentStockQuantity = product.StockQuantity,
                        QuantityChanged = 0,
                        ChangeType = InventoryChangeType.Reserved,
                        ReferenceId = referenceId,
                        ReferenceType = referenceType,
                        Notes = $"Reserved {quantity} units for {referenceType} {referenceId}",
                        CreatedAt = DateTime.UtcNow
                    };

                    var updatedProduct = await _productRepository.UpdateAsync(product.Id, product);
                    var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

                    if (createdInventory == null || updatedProduct == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }

                    return await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw; // Let EF retry if it's a transient exception
                }
            });
        }


    }
}
