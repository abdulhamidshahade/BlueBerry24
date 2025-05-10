

using AutoMapper;
using BlueBerry24.Application.Dtos.StockDtos;
using BlueBerry24.Application.Services.Interfaces.StockServiceInterfaces;
using BlueBerry24.Domain.Entities.StockEntities;
using BlueBerry24.Domain.Repositories.StockInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text;

namespace BlueBerry24.Application.Services.Concretes.StockServiceConcretes
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public StockService(IStockRepository stockRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public async Task<StockDto> CreateStockAsync(CreateStockDto stockDto)
        {
            if(stockDto == null)
            {
                return null;
            }

            //if(string.IsNullOrEmpty(stockDto.ProductId))
            //{
            //    throw new ArgumentException("The ProductId property is null or empty.", nameof(stockDto.ProductId));
            //}

            //if (string.IsNullOrEmpty(stockDto.ShopId))
            //{
            //    throw new ArgumentException("The ShopId property is null or empty.", nameof(stockDto.ShopId));
            //}

            if(stockDto.Quantity <= 0)
            {
                return null;
            }

            //if(!await _productService.ExistsByShopIdAsync(stockDto.ProductId, stockDto.ShopId))
            //{
            //    throw new InvalidOperationException("The product does not exist in the specified shop.");
            //}

            var existingStock = await _stockRepository.GetStockByProductId(stockDto.ProductId);

            if(existingStock != null)
            {
                existingStock.Quantity += stockDto.Quantity;
                var updatedStock = await _stockRepository.UpdateStockByIdAsync(existingStock.Id, existingStock);

                var mappedToDto = _mapper.Map<StockDto>(updatedStock);
                return mappedToDto;
            }
            else
            {
                var mappedStock = _mapper.Map<Stock>(existingStock);
                var addedStock = await _stockRepository.CreateStockAsync(mappedStock);


                var mappedToDto = _mapper.Map<StockDto>(addedStock);
                return mappedToDto;
            }
        }

        
       
        public async Task<bool> UpdateStockByIdAsync(int id, UpdateStockDto stockDto)
        {
            if(stockDto == null)
            {
                return false;
            }

            //if (string.IsNullOrEmpty(stockDto.ProductId))
            //{
            //    throw new ArgumentException("The productId parameter is null or empty", nameof(stockDto.ProductId));
            //}

            //if (string.IsNullOrEmpty(stockDto.ShopId))
            //{
            //    throw new ArgumentException("The shopId parameter is null or empty", nameof(stockDto.ShopId));
            //}

            //if(!await _productService.ExistsByShopIdAsync(stockDto.ProductId, stockDto.ShopId))
            //{
            //    throw new InvalidOperationException("The product does not exist in the specified shop.");
            //}

            var existingStock = await _stockRepository.GetStockByIdAsync(id);

            if(existingStock != null)
            {
                if(stockDto.Quantity < 0)
                {
                    if(stockDto.Quantity >= -existingStock.Quantity)
                    {
                        existingStock.Quantity += stockDto.Quantity;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    existingStock.Quantity += stockDto.Quantity;
                }
            }
            else
            {
                return false;
            }

            var updatedStock = await _stockRepository.UpdateStockByIdAsync(id, existingStock);
            return updatedStock;
        }



        public async Task<bool> DeleteStockByIdAsync(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    throw new ArgumentException("The id parameter is null or empty", nameof(id));   
            //}

            var existingStock = await _stockRepository.GetStockByIdAsync(id);

            if(existingStock != null)
            {
                var deletedStock = await _stockRepository.DeleteStockByIdAsync(existingStock);
                return deletedStock;
            }

            else
            {
                return false;
            }

        }



        public async Task<StockDto> GetStockByIdAsync(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    throw new ArgumentException("The id property is null or empty", nameof(id));
            //}

            //_logger.LogInformation($"Fetching stock item with id: {id}");

            var stock = await _stockRepository.GetStockByIdAsync(id);

            if(stock == null)
            {
                return null;
            }

            var stockToDto = _mapper.Map<StockDto>(stock);

            return stockToDto;
        }



        //public async Task<List<Stock>> GetStocksByShopIdAsync(string shopId)
        //{
        //    if (string.IsNullOrEmpty(shopId))
        //    {
        //        throw new ArgumentException("The shopId parameter is null or empty", nameof(shopId));
        //    }

        //    _logger.LogInformation($"Fetching stocks by shopId: {shopId}");
        //    var stocks = await _context.Stocks.Where(i => i.ShopId == shopId).ToListAsync();

        //    if(stocks == null)
        //    {
        //        throw new InvalidOperationException("The stock items do not exist.");
        //    }

        //    return stocks;
        //}
        

        
        //public async Task CheckStock(string productId, string shopId, int quantity)
        //{
    
        //    if (quantity <= 5)
        //    {
        //        await PublishStockEmptyEvent(new { ProductId = productId, ShopId = shopId, Message = "Quantity is less than 5 items" });
        //    }
        //}



        public async Task<bool> IncreaseByItemAsync(int productId)
        {
            //if(string.IsNullOrEmpty(productId))
            //{
            //    throw new ArgumentException("The productId parameter is null or empty", nameof(productId));
            //}

            //if (string.IsNullOrEmpty(shopId))
            //{
            //    throw new ArgumentException("The shopId parameter is null or empty", nameof(shopId));
            //}

            //_logger.LogInformation($"Fetching stock by productId: {productId} and shopId: {shopId}");


            var item = await _stockRepository.GetStockByProductId(productId);

            if(item == null)
            {
                return false;
            }

            item.Quantity++;
            return await _stockRepository.UpdateStockAsync(item);
        }



        public async Task<bool> DecreaseByItemAsync(int productId)
        {
            //if(string.IsNullOrEmpty(shopId))
            //{
            //    throw new ArgumentException("The shopId parameter is null or empty", nameof(shopId));
            //}

            //if (string.IsNullOrEmpty(productId))
            //{
            //    throw new ArgumentException("The productId parameter is null or empty", nameof(productId));
            //}

            //_logger.LogInformation($"Fetching stock by productId: {productId} and shopId: {shopId}");


            var item = await _stockRepository.GetStockByProductId(productId);

            if(item == null)
            {
                return false;
            }

            item.Quantity--;
            return await _stockRepository.UpdateStockAsync(item);
        }

        public async Task<bool> IsStockAvailableAsync(int productId)
        {
            var isAvaiable = await _stockRepository.IsStockAvailableAsync(productId);

            return isAvaiable;
        }

        public async Task<List<StockDto>> GetStocks()
        {
            var stocks = await _stockRepository.GetStocks();

            if(stocks == null)
            {
                return null;
            }

            var mappedStocks = _mapper.Map<List<StockDto>>(stocks);

            return mappedStocks;
        }
    }
}