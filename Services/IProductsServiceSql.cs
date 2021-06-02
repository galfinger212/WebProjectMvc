using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProject.Services
{
    public interface IProductsServiceSql
    {
        Task<List<ProductModel>> GetAllProductsAsync();
        Task<ProductModel> GetProductByIdAsync(long id);
        Task<List<ProductModel>> GetAllByUserIdAsync(long id);
        void PostAdd(ProductModel product);
        void AddToCart(ProductModel product);
        void RemoveFromCart(ProductModel product);
        void BuyProduct(ProductModel product);
        List<ProductModel> RemoveAllForgottenProducts();
    }
}