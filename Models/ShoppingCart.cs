using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }
        public string ApplicationUserId { get; set; }
        public ICollection<ProductsInShoppingCart> ProductsInShoppingCart { get; set; }


    }
}
