using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Domain.DTO;
using EShop.Domain.DomainModels;
using EShop.Service.Interface;


namespace EShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService _productService)
        {
            this._productService = _productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(_productService.GetAllProducts());
        }

        //You can either use the synchroneous method like: public IActionResult .. and then  _context....().FirstOrDefault();
        //^this is good for lower quantities of data
        //or you can use the async version: public async Task<IActionResult> ... and then await _context....().FirstOrDefaultAsync();
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = _productService.GetDetailsForProduct(productId);
            var model = new AddToShoppingCartDto();
            model.SelectedProduct = product;
            model.ProductId = product.Id;
            model.Quantity = 0;

            // simplified version for instatiating a new object 
            //var model = new AddToShoppingCartDto
            //{
            //    SelectedProduct = product,
            //    ProductId = product.ProductId,
            //    Quantity = 0

            //};

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(AddToShoppingCartDto model)
        {
            //gets the Id of the currently logged in user 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _productService.AddToShoppingCart(model, userId);
            return RedirectToAction("Index");
        }
        

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //id??0 if id is null it will add 0 to the funct
            var product = _productService.GetDetailsForProduct(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }
            return View("Index", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdateExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _productService.GetDetailsForProduct(id) != null;
        }
    }
}
