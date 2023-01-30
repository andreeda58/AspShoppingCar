using AspProject_Entities.Enums;
using AspProject_Entities.Models;
using AspProject_Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }
        public IActionResult NewAd() => View();


        [HttpPost]
        public IActionResult AddProduct(Product product, List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
                return View("NewAd", product);

            for (int i = 0; i < Images.Count; i++)
            {
                if (Images[i].Length > 0)
                {
                    using MemoryStream ms = new MemoryStream();
                    Images[i].CopyTo(ms);
                    product.GetType().GetProperty($"Picture{i + 1}").SetValue(product, ms.ToArray());
                }
            }

            //cambia los datos a llenar 
            product.State = ItemState.UnSold;
            string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
            product.Seler = product.Buyer = _userService.GetUser(UsernamePassword[0], UsernamePassword[1]);
            product.LastModified = product.Date = DateTime.Now;
            _productService.AddProduct(product);
            return RedirectToAction("Index", "Master");
        }



        public IActionResult AddToCart(int id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AspProjectCookie"))
            {
                string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
                _productService.AddProductToCart(id, _userService.GetUser(UsernamePassword[0], UsernamePassword[1]));
            }//if have guest cart I save the cookies again with the id
            else if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))
                HttpContext.Response.Cookies.Append("AspProjectGuestCart", HttpContext.Request.Cookies["AspProjectGuestCart"] + $",{id}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            else
                HttpContext.Response.Cookies.Append("AspProjectGuestCart", $"{id}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            return RedirectToAction("Index", "Master");
        }
        public IActionResult ProductDetails(int id)
        {
            return View(_productService.GetProductByID(id));
        }







        public IActionResult RemoveFromCart(int id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AspProjectCookie"))
            {
                string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
                _productService.RemoveFromCart(id, _userService.GetUser(UsernamePassword[0], UsernamePassword[1]));
                return View("Cart", _productService.GetCart(_userService.GetUser(UsernamePassword[0], UsernamePassword[1])));
            }
            else
            {
                List<string> AnonymusCartItems = HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList();
                AnonymusCartItems.Remove(id.ToString());
                if (AnonymusCartItems.Count == 0)
                {
                    HttpContext.Response.Cookies.Delete("AspProjectGuestCart");
                    return RedirectToAction("Index", "Master");
                }
                else
                {//I create new cookies for the remaining products
                    HttpContext.Response.Cookies.Append("AspProjectGuestCart", string.Join(',', AnonymusCartItems), new CookieOptions() { Expires = DateTime.Now.AddDays(3) });

                    //send the rest products to the view
                    List<int> ProductIDs = new List<int>();
                    AnonymusCartItems.ForEach(idstring => ProductIDs.Add(int.Parse(idstring)));
                    return View("Cart", _productService.GetCart(ProductIDs));
                }
            }
        }
        public IActionResult Cart()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AspProjectCookie"))
            {
                string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
                return View(_productService.GetCart(_userService.GetUser(UsernamePassword[0], UsernamePassword[1])));//send the user
            }
            else
            {
                if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))
                {
                    List<int> ProductIDs = new List<int>();
                    HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(idstring => ProductIDs.Add(int.Parse(idstring))); ;
                    return View(_productService.GetCart(ProductIDs));
                }
                else
                {
                    return View(Enumerable.Empty<Product>());
                }
            }
        }
        public async Task<IActionResult> Purchase()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AspProjectCookie"))
            {
                string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
                await _productService.Purchase(_userService.GetUser(UsernamePassword[0], UsernamePassword[1]));
            }
            else if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))
            {
                List<int> ProductIDs = new List<int>();
                HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(idstring => ProductIDs.Add(int.Parse(idstring))); ;
                await _productService.Purchase(ProductIDs);
            }
            return RedirectToAction("Thanks", "Product");
        }
        public IActionResult Thanks() => View();
       
    }
}
