using AspProject_Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject_Services.Interfaces
{
    public interface IProductService
    {
        void AddProduct(Product product);
        IEnumerable<Product> GetCart(List<int> productIDs);
        IEnumerable<Product> GetAllUnSoldProducts(List<int> productsInGuestCart);
        Task Purchase(User user);
        Task Purchase(List<int> productIDs);
        IEnumerable<Product> GetAllUnSoldProducts();
        Product GetProductByID(int id);
        void AddProductToCart(int id, User user);
        IEnumerable<Product> GetCart(User user);
        void RemoveFromCart(int id, User user);
    }
}
