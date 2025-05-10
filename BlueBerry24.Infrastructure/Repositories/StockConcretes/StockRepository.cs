using BlueBerry24.Domain.Entities.StockEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.StockInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.StockConcretes
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public StockRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public Task CheckStock(int productId, int shopId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> DecreaseByItemAsync(int productId, int shopId)
        {
            var stock = await _context.Stocks.Where(fk => fk.ProductId == productId)
                .FirstOrDefaultAsync();

            if (stock == null) return false;

            stock.Quantity--;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStockByIdAsync(Stock stock)
        {
            _context.Stocks.Remove(stock);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            return stock;
        }

        public async Task<Stock> GetStockByProductId(int productId)
        {
            var stock = await _context.Stocks.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return stock;
        }

        public async Task<List<Stock>> GetStocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return stocks;
        }

        //public async Task<List<Stock>> GetStocksByShopIdAsync(int shopId)
        //{
        //    var stocks = await _context.Stocks.Where(s => s.ShopId == shopId).ToListAsync();
        //    return stocks;
        //}

        public async Task<bool> IncreaseByItemAsync(int productId, int shopId)
        {
            var stock = await _context.Stocks.Where(pk => pk.ProductId == productId)
                .FirstOrDefaultAsync();

            if (stock == null) return false;

            stock.Quantity++;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsStockAvailableAsync(int productId)
        {
            var stock = await _context.Stocks.Where(pk => pk.ProductId == productId)
                .FirstOrDefaultAsync();

            if (stock == null) return false;

            if(stock.Quantity == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateStockAsync(Stock stock)
        {
            var stockDb = await _context.Stocks.FindAsync(stock.Id);

            if(stockDb == null)
            {
                return false;
            }

            stockDb.Quantity = stock.Quantity;
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<bool> UpdateStockByIdAsync(int id, Stock stock)
        {
            var stockModel = await _context.Stocks.FindAsync(id);

            stockModel.Quantity = stock.Quantity;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
