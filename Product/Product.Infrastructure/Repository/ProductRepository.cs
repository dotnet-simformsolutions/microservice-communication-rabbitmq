using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDBContext _ecommerceDBContext;
        public ProductRepository(EcommerceDBContext ecommerceDBContext)
        {
            _ecommerceDBContext = ecommerceDBContext;
        }
        public async Task AddProduct(Domain.Entities.Product product)
        {
            await _ecommerceDBContext.Products.AddAsync(product);
            await _ecommerceDBContext.SaveChangesAsync();
        }

        public async Task EditProduct(Domain.Entities.Product product)
        {
            _ecommerceDBContext.Products.Update(product);
            await _ecommerceDBContext.SaveChangesAsync();
        }

        public async Task<List<Domain.Entities.Product>> GetAllProducts()
        {
            return await _ecommerceDBContext.Products.ToListAsync();
        }

        public async Task<Domain.Entities.Product> GetById(int id)
        {
            return await _ecommerceDBContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
