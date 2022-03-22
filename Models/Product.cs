using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId{ get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int Rating { get; set; }
        public ICollection<ProductsInShoppingCart> ProductsInShoppingCart { get; set; }
    }
}
