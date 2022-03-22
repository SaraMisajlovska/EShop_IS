using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class ProductsInShoppingCart
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("CartId")]
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }

    }
}
