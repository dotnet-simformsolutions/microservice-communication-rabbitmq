using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository.IRepository
{
    public interface IProductRepository
    {
        Task AddProduct(Domain.Entities.Product product);
        Task EditProduct(Domain.Entities.Product product);
        Task<Domain.Entities.Product> GetById(int id);
        Task<List<Domain.Entities.Product>> GetAllProducts();
    }
}
