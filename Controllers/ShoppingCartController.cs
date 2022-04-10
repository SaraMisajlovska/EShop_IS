using EShop.Data;
using EShop.Models;
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
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //we have to use include to include the relationships user has
            var user = _context.Users.Where(u => u.Id == userId).Include("UserShoppingCart.ProductsInShoppingCart").Include("UserShoppingCart.ProductsInShoppingCart.Product").FirstOrDefault();
            var userShoppingCart = user.UserShoppingCart;

            var productList = userShoppingCart.ProductsInShoppingCart.Select(p => new
            {
                Quantity = p.Quantity,
                ProductPrice = p.Product.ProductPrice
            });

            float totalPrice = 0;
            foreach (var item in productList)
            {
                totalPrice += item.ProductPrice * item.Quantity;
            }

            ShoppingCartDto model = new ShoppingCartDto
            {
                TotalPrice = totalPrice,
                ProductsInShoppingCart = userShoppingCart.ProductsInShoppingCart.ToList()
            };

            return View(model);
        }

        //here we have to pass id (it cannot be named anything else) because its hard coded in some class for routing and core expects it
        public IActionResult DeleteFromShoppingCart (int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //we have to use include to include the relationships user has
            var user = _context.Users.Where(u => u.Id == userId).Include("UserShoppingCart.ProductsInShoppingCart").Include("UserShoppingCart.ProductsInShoppingCart.Product").FirstOrDefault();
            var userShoppingCart = user.UserShoppingCart;

            var remove = userShoppingCart.ProductsInShoppingCart.Where(p => p.ProductId == id).FirstOrDefault();
            userShoppingCart.ProductsInShoppingCart.Remove(remove);
            _context.Update(userShoppingCart);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //we have to use include to include the relationships user has
            var user = _context.Users.Where(u => u.Id == userId).Include("UserShoppingCart.ProductsInShoppingCart").Include("UserShoppingCart.ProductsInShoppingCart.Product").FirstOrDefault();
            var userShoppingCart = user.UserShoppingCart;

            Order newOrder = new Order
            {
                UserId = user.Id,
                OrderedBy = user
            };

            _context.Add(newOrder);
            _context.SaveChanges();

            List<ProductInOrder> productInOrder = userShoppingCart.ProductsInShoppingCart.Select(p => new ProductInOrder
            {
                Product = p.Product,
                ProductId = p.ProductId,
                Order = newOrder,
                OrderId = newOrder.OrderId
            }).ToList();

            foreach (var item in productInOrder)
            {
                _context.Add(item);
            }
            user.UserShoppingCart.ProductsInShoppingCart.Clear();
            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
