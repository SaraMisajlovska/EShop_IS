using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        //this part here is dependency injection like in Java
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService _shoppingCartService)
        {
            this._shoppingCartService = _shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _shoppingCartService.GetShoppingCartInfo(userId);
            return View(model);
        }

        //here we have to pass id (it cannot be named anything else) because its hard coded in some class for routing and core expects it
        public IActionResult DeleteFromShoppingCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //we have to use include to include the relationships user has
            _shoppingCartService.DeleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index");

        }

        public IActionResult OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.OrderNow(userId);
            return RedirectToAction("Index");

        }
    }
}
