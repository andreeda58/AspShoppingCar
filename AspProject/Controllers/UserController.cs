using AspProject_Entities.Models;
using AspProject_Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }
        public IActionResult SignUp()///registrer user
        {
            return View();
        }//Inscription


        [HttpPost]
        public IActionResult SignIn(SignInModel signInModel)
        {
            if (!ModelState.IsValid)//is error exist
            {
                TempData["LogInError"] = "Username or Pasword not filled approprietly";
                return RedirectToAction("Index", "Master");
            }
            if (_userService.GetUser(signInModel.Username, signInModel.Password, out User user))//If the user exist
            {
                HttpContext.Response.Cookies.Append("AspProjectCookie", $"{signInModel.Username},{signInModel.Password}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
                if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))// if exist AspProjectGuestCart delete/// sis existe carro de visitante
                {
                    HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(idstring => _productService.AddProductToCart(int.Parse(idstring), user)); ;
                    HttpContext.Response.Cookies.Delete("AspProjectGuestCart");
                }
            }
            else
            {
                TempData["LogInError"] = "Incorrect Username or Password";
                return RedirectToAction("Index", "Master");
            }
            return RedirectToAction("Index", "Master");
        }
        public IActionResult SignOut()
        {
            HttpContext.Response.Cookies.Delete("AspProjectCookie");
            return RedirectToAction("Index", "Master");
        }


        public IActionResult UsernameCheck(string username)
        {
            if (_userService.CheckIfUserExists(username))
                return StatusCode(200);
            else return StatusCode(404);
        }
        public IActionResult PasswordCheck(string username, string password)
        {
            if (_userService.CheckIfUserExists(username))
                if (_userService.CheckIfPasswordMatch(username, password))
                    return StatusCode(200);
            return StatusCode(404);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
                return View("SignUp", user);
            else if (_userService.CheckIfUserExists(user.UserName))
            {
                ViewBag.UserAlreadyExistsError = "User already Exists";
                return View("SignUp", user);
            }
            else
            {
                _userService.AddUser(user);
                HttpContext.Response.Cookies.Append("AspProjectCookie", $"{user.UserName},{user.Password}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
              
                
                if (HttpContext.Request.Cookies.ContainsKey("AspProjectGuestCart"))// se borra si se tiene cart de visitante//
                {
                    HttpContext.Request.Cookies["AspProjectGuestCart"].Split(',').ToList().ForEach(idstring => _productService.AddProductToCart(int.Parse(idstring), user)); ;
                    HttpContext.Response.Cookies.Delete("AspProjectGuestCart");
                }
            }
            return RedirectToAction("Index", "Master");
        }


        public IActionResult UpdateUser()//incribir usuario o editar get
        {
            string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
            return View(_userService.GetUser(UsernamePassword[0], UsernamePassword[1]));
        }


        [HttpPost]
        public IActionResult UpdateUser(User user)
        {

            //revisar si el usuario existe
            _userService.UpdateUser(user);
            string[] UsernamePassword = HttpContext.Request.Cookies["AspProjectCookie"].Split(',');
            HttpContext.Response.Cookies.Append("AspProjectCookie", $"{UsernamePassword[0]},{user.Password}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            return RedirectToAction("Index", "Master");
        }
    }
}
