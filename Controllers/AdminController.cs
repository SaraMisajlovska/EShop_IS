using EShop.Domain.DomainModels;
using EShop.Domain.Identity;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ShopApplicationUser> _userManager;

        public AdminController(IOrderService orderService,
            UserManager<ShopApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpPost("[action]")]
        public Order GetOrderDetails(BaseEntity model)
        {
            return _orderService.GetOrderDetails(model);
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;
            foreach (var user in model)
            {
                var userCheck = _userManager.FindByEmailAsync(user.Email).Result;
                if (userCheck == null)
                {
                    var newUser = new ShopApplicationUser
                    {
                        Name = "",
                        Surname = "",
                        Address = "",
                        UserName = user.Email,
                        NormalizedUserName = user.Email,
                        Email = user.Email,
                        UserShoppingCart = new ShoppingCart()
                    };

                    var result = _userManager.CreateAsync(newUser, user.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return status;
        }
    }
}
