using AspProject_Entities.Models;
using AspProject_Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class MasterController : Controller
    {
        private readonly IProductService _productService;
        public MasterController(IProductService productService) => _productService = productService;
       

        public IActionResult Index()
        {
            
            if (TempData.ContainsKey("Sorting"))
            {
                List<Product> ProductList;

                //In the case that there are products in the cart, I do not show them in the sorting
                if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))
                {
                    List<int> ProductsInAnnonymusCart = new List<int>();
                    HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(id => ProductsInAnnonymusCart.Add(int.Parse(id)));
                    ProductList = _productService.GetAllUnSoldProducts(ProductsInAnnonymusCart).ToList();//get the unsold products 
                }
                else
                    ProductList = _productService.GetAllUnSoldProducts().ToList();

                ////sort
                if ((string)TempData["Sorting"] == "Title")
                    ProductList.Sort(delegate (Product a, Product b) { return a.Title.CompareTo(b.Title); });
                else
                    ProductList.Sort(delegate (Product a, Product b) { return a.Date.CompareTo(b.Date); });

                return View(ProductList);
            }



            if (TempData.ContainsKey("LogInError"))
                ViewBag.LogInError = TempData["LogInError"];

            //In the case that there are products in the guest cart
            if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))
            {
                List<int> ProductsInAnnonymusCart = new List<int>();
                HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(idstring => ProductsInAnnonymusCart.Add(int.Parse(idstring)));
                return View(_productService.GetAllUnSoldProducts(ProductsInAnnonymusCart));
            }
            else return View(_productService.GetAllUnSoldProducts());
           
        }
        public IActionResult AboutUs() => View();


    }
}
