using AspProject_DataBase.Context;
using AspProject_Entities.Models;
using AspProject_Entities.Enums;
using AspProject_Services.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AspProject_Services.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext Context)
        {
            _context = Context;
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void AddProductToCart(int id, User user)
        {
            Product product = _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.Id == id).FirstOrDefault();
            product.LastModified = DateTime.Now;
            product.Buyer = user;
            product.State = ItemState.InCart;
            _context.SaveChanges();
        }
      
       
      
        public void RemoveFromCart(int id, User user)
        {
            Product product = _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.Id == id).FirstOrDefault();
            product.LastModified = DateTime.Now;
            product.Buyer = product.Seler;
            product.State = ItemState.UnSold;
            _context.SaveChanges();
        }

        public Product GetProductByID(int id)
       => _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.Id == id).FirstOrDefault();
        public IEnumerable<Product> GetAllUnSoldProducts()
        => _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.State == ItemState.UnSold).ToList();
        public IEnumerable<Product> GetAllUnSoldProducts(List<int> productsInAnnonymusCart)
        {
            return _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.State == ItemState.UnSold && !productsInAnnonymusCart.Contains(p.Id)).ToList();
        }
        public IEnumerable<Product> GetCart(User user)
        => _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.Buyer == user && p.State == ItemState.InCart).ToList();
        public IEnumerable<Product> GetCart(List<int> productsInAnnonymusCart)
        => _context.Products.Include(p => p.Seler).Include(p => p.Buyer).Where(p => p.State == ItemState.UnSold && productsInAnnonymusCart.Contains(p.Id)).ToList();




        private async Task Purchase(IEnumerable<Product> productsToPurchase, User user = null)
        {
            await _context.Products.Where(product => productsToPurchase.Contains(product)).ForEachAsync((product) =>
            {
                product.State = ItemState.Sold;
                if (user != null) product.Buyer = user;
                else product.Buyer = null;
            });
            _context.SaveChanges();
        }
        public async Task Purchase(User user) => await Purchase(GetCart(user), user);
        public async Task Purchase(List<int> productsInAnnonymusCart) => await Purchase(GetCart(productsInAnnonymusCart));
    }
}
