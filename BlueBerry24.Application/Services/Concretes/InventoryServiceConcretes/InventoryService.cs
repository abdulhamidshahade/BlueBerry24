using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Domain.Entities.InventoryEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.InventoryInterfaces;
using BlueBerry24.Domain.Repositories.ProductInterfaces;

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
            return product != null && (product.StockQuantity - product.ReservedStock) >= quantity;
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

            // Increase stock
            product.StockQuantity += quantity;
            product.UpdatedAt = DateTime.UtcNow;

            var updatedProduct = await _productRepository.UpdateAsync(productId, product);


            var inventoryLog = new InventoryLog
            {

                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = quantity, // Positive because it's added
                ChangeType = InventoryChangeType.Restock,
                Notes = notes,
                CreatedAt = DateTime.UtcNow,
                PerformedByUserId = performedByUserId
            };

            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

            if(createdInventory == null)
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
                : InventoryChangeType.StockAdjustment;

            // Update stock quantity
            product.StockQuantity = newQuantity;

            // If new stock is less than reserved, adjust reserved too
            if (product.StockQuantity < product.ReservedStock)
            {
                product.ReservedStock = product.StockQuantity;
            }

            product.UpdatedAt = DateTime.UtcNow;

            // Create inventory log entry
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

            if(product.StockQuantity < quantity)
            {
                return false;
            }

            int reservedDeduction = quantity;
            product.ReservedStock -= reservedDeduction;


            product.StockQuantity -= reservedDeduction;

            product.UpdatedAt = DateTime.UtcNow;

            // Create inventory log entry
            var inventoryLog = new InventoryLog
            {
                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = -quantity, // Negative because it's deducted
                ChangeType = InventoryChangeType.Purchase,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                Notes = $"Purchased {quantity} units via {referenceType} {referenceId}",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            
             
            var updatedProduct = await _productRepository.UpdateAsync(product.Id, product);

            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);
            if (createdInventory == null || updatedProduct == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            return await _unitOfWork.CommitTransactionAsync();

            
            //if (product.StockQuantity <= product.LowStockThreshold)
            //{
            //    In a real implementation, could trigger notification here
            //    await _notificationService.SendLowStockNotificationAsync(product);
            //}
        }

        public async Task<List<InventoryLog>> GetInventoryHistoryAsync(int productId, int limit = 50)
        {
            var inventoryHistory = await _inventoryRepository.GetInventoryHistoryAsync(productId, limit);

            return inventoryHistory;
        }

        public Task<List<Product>> GetLowStockProductsAsync(int limit = 50)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductWithStockInfoAsync(int productId)
        {
            throw new NotImplementedException();
        }


        public Task ProcessStockNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReleaseReservedStockAsync(int productId, int quantity, int referenceId, string referenceType)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            // Cannot release more than what's reserved
            if (product.ReservedStock < quantity)
            {
                quantity = product.ReservedStock; // Release what we can
            }

            // Release the reserved stock
            product.ReservedStock -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // Create inventory log entry
            var inventoryLog = new InventoryLog
            {
                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = quantity, // Positive because it's released back to available
                ChangeType = InventoryChangeType.ReleaseReservation,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                Notes = $"Released {quantity} units from {referenceType} {referenceId}",
                CreatedAt = DateTime.UtcNow
            };

            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

            if (createdInventory == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ReserveStockAsync(int productId, int quantity, 
            int referenceId, string referenceType)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            int availableStock = product.StockQuantity - product.ReservedStock;

            if (availableStock < quantity)
            {
                return false;
            }


            product.ReservedStock += quantity;
            product.UpdatedAt = DateTime.UtcNow;

            var inventoryLog = new InventoryLog
            {
                ProductId = productId,
                CurrentStockQuantity = product.StockQuantity,
                QuantityChanged = -quantity,
                ChangeType = InventoryChangeType.Reserved,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                Notes = $"Reserved {quantity} units for {referenceType} {referenceId}",
                CreatedAt = DateTime.UtcNow
            };


            var createdInventory = await _inventoryRepository.CreateInventory(inventoryLog);

            if(createdInventory == null)
            {
                return false;
            }

            return true;
        }



    }
}
