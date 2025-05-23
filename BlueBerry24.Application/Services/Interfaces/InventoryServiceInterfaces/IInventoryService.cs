using BlueBerry24.Domain.Entities.InventoryEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces
{
    public interface IInventoryService
    {
        Task<bool> IsInStockAsync(int productId, int quantity);
        Task<bool> ReserveStockAsync(int productId, int quantity, int referenceId, string referenceType);
        Task<bool> ReleaseReservedStockAsync(int productId, int quantity, int referenceId, string referenceType);
        Task<bool> ConfirmStockDeductionAsync(int productId, int quantity, int referenceId, string referenceType);
        Task<bool> AddStockAsync(int productId, int quantity, string notes, int? performedByUserId);
        Task<bool> AdjustStockAsync(int productId, int newQuantity, string notes, int? performedByUserId);
        Task<Product> GetProductWithStockInfoAsync(int productId);
        Task<List<Product>> GetLowStockProductsAsync(int limit = 50);
        Task<List<InventoryLog>> GetInventoryHistoryAsync(int productId, int limit = 50);
        Task ProcessStockNotificationsAsync();
    }
}
