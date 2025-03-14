using AutoMapper;
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Models;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueBerry24.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly ILogger<ShoppingCartsController> _logger;
        private readonly IMapper _mapper;
        public ShoppingCartsController(ILogger<ShoppingCartsController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetCurrentUserCart()
        {
            var userId = User.FindFirst(ClaimTypes.Name).Value;

            return Ok();
        }
    }
}
