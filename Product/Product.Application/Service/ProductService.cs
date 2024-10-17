using Product.Application.Service.IService;
using Product.Infrastructure.Repository.IRepository;

namespace Product.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task AddProduct(Domain.Entities.Product product)
        {
            await _productRepository.AddProduct(product);
        }

        public async Task EditProduct(Domain.Entities.Product product)
        {
            await _productRepository.EditProduct(product);
        }

        public async Task<List<Domain.Entities.Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Domain.Entities.Product> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }
    }
}
