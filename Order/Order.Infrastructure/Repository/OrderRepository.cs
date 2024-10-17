using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Order.Domain.Entities;
using Order.Infrastructure.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDBContext _ecommerceDBContext;
        public OrderRepository(EcommerceDBContext ecommerceDBContext)
        {
            _ecommerceDBContext = ecommerceDBContext;
        }
        public async Task<List<Domain.Entities.Order>> GetOrderById(int id)
        {
            return await _ecommerceDBContext.Orders.Where(x => x.Id == id).ToListAsync();

        }

        public async Task<List<Domain.Entities.Order>> GetOrderByUser(string userEmail)
        {
            return await _ecommerceDBContext.Orders.Where(x => x.UserEmail == userEmail).ToListAsync();
        }

        public async Task<bool> OrderNow(Domain.Entities.Order order)
        {
            try
            {
                var alloworder = true;
                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = 0;
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync("https://localhost:44336/api/product/GetProductQuantity?id=" + orderItem.ProductId);

                        if (response.IsSuccessStatusCode)
                        {
                            quantity = Convert.ToInt16(await response.Content.ReadAsStringAsync());
                        }
                    }
                    if (quantity < orderItem.Quantity)
                    {
                        alloworder = false;
                        break;
                    }
                }
                if (alloworder)
                {
                    _ecommerceDBContext.Orders.Add(order);
                    await _ecommerceDBContext.SaveChangesAsync();
                }
                return alloworder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
