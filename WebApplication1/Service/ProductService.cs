using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class ProductService
    {
        private readonly List<Product> products = [];
        public IEnumerable<Product> GetAll() => products;
        public Product? GetById(Guid id) => products.FirstOrDefault(p => p.Id == id);  
        public Product Create(Product product)
        {
            product.Id = Guid.NewGuid();
            products.Add(product);
            return product;
        }
        public bool Updated(Guid id, Product updatedProduct)
        {
            Product product = GetById(id);
            if (product is null) return false;

            product.Name = updatedProduct.Name;
            product.UpdatedDate = DateTime.Now;

            return true;
        }

        public bool Delete(Guid id)
        {
            Product product = GetById(id);
            if (product is null) return false;

            products.Remove(product);
            return true;
        }
    }
}
