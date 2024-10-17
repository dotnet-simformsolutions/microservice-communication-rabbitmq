using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;
using Order.Application.Service.IService;
using System.Net;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        private readonly IMapper mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _OrderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> OrderNow(OrderDto productdto)
        {
            var product = mapper.Map<Domain.Entities.Order>(productdto);
            try
            {
                await _OrderService.OrderNow(product);
            }
            catch(Exception ex)
            {
                return Ok();
            }
            return Ok();
        }

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            await _OrderService.GetOrderById(id);
            return Ok();
        }
        [HttpGet("GetOrderByUser")]
        public async Task<IActionResult> GetOrderByUser(string userEmail)
        {
            await _OrderService.GetOrderByUser(userEmail);
            return Ok();
        }
        
    }
}
