using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Service.IService
{
    public interface IOrderService
    {
        Task OrderNow(Domain.Entities.Order order);
        Task<List<Domain.Entities.Order>> GetOrderById(int id);

        Task<List<Domain.Entities.Order>> GetOrderByUser(string userEmail);

    }
}
