using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<bool> OrderNow(Domain.Entities.Order order);
        Task<List<Domain.Entities.Order>> GetOrderById(int id);
        Task<List<Domain.Entities.Order>> GetOrderByUser(string userEmail);
    }
}
