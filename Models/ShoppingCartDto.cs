using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class ShoppingCartDto
    {
        public List<ProductsInShoppingCart> ProductsInShoppingCart { get; set; }
        public float TotalPrice { get; set; }

    }
}
