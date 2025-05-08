using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.StockDtos;
using BlueBerry24.Application.Services.Interfaces.StockServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        private readonly ILogger<StocksController> _logger;

        public StocksController(IStockService stockService,
                                ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> GetStockById(int id)
        {

            var stock = await _stockService.GetStockByIdAsync(id);

            if (stock == null)
            {
                _logger.LogError($"Error occured in {nameof(GetStockById)}");

                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error occured while retriving the stock"
                });
            }

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = $"Stock #{stock.Id} found",
                Data = stock
            });
        }




        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> UpdateStockById(int id, UpdateStockDto stockDto)
        {
            try
            {
                var updatedStock = await _stockService.UpdateStockByIdAsync(id, stockDto);

                if (!updatedStock)
                {
                    return BadRequest(new ResponseDto
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "The stock was not updated successfully"
                    });
                }

                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusMessage = "The stock updated successfully",
                    StatusCode = 200
                });
            }
            catch(ArgumentException ex)
            {
                return StatusCode(500, new ResponseDto
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    StatusMessage = ex.Message
                });
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode(500, new ResponseDto
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    StatusMessage = ex.Message
                });
            }
        }


        //[HttpGet]
        //[Route("shop/{shopId}")]
        //public async Task<ActionResult<ResponseDto>> GetStocksByShopId(string shopId)
        //{
        //    try
        //    {
        //        var stocks = await _stockService.GetStocksByShopIdAsync(shopId);

        //        if(stocks == null)
        //        {
        //            return NotFound(new ResponseDto
        //            {
        //                IsSuccess = false,
        //                StatusCode = 404,
        //                StatusMessage = $"No stock found for shop with id: {shopId}"
        //            });
        //        }

        //        return Ok(new ResponseDto
        //        {
        //            IsSuccess = true,
        //            StatusMessage = "Stocks found",
        //            Data = _mapper.Map<List<Stock>>(stocks),
        //            StatusCode = 200
        //        });
        //    }
        //    catch(InvalidOperationException ex)
        //    {
        //        return StatusCode(500, new ResponseDto
        //        {
        //            IsSuccess = false,
        //            StatusCode = 500,
        //            StatusMessage = ex.Message,
        //        });
        //    }
        //    catch(ArgumentException ex)
        //    {
        //        return StatusCode(500, new ResponseDto
        //        {
        //            IsSuccess = false,
        //            StatusCode = 500,
        //            StatusMessage = ex.Message
        //        });
        //    }
        //}

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> DeleteStockById(int id)
        {
            try
            {
                var stock = await _stockService.DeleteStockByIdAsync(id);

                if (!stock)
                {
                    return NotFound(new ResponseDto
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = $"The stock with id: {id} was not found"
                    });
                }
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Stock with id: {id} deleted",
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Error occured in {nameof(DeleteStockById)}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error occured in {nameof(DeleteStockById)}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = ex.Message
                });
            }

        }
    }
}
