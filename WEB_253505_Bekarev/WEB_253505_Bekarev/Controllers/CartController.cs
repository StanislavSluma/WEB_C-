using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Services.ProductService;
using WEB_253505_Bekarev.Extensions.HostingExtensions;

namespace WEB_253505_Bekarev.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;
        public CartController(IProductService product_service, Cart cart)
        {
            _productService = product_service;
            _cart = cart;
        }

        // GET: CartController
        public ActionResult Index()
        {
            return View(_cart);
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                cart.AddToCart(data.Data);
                HttpContext.Session.Set<Cart>("cart", cart);
            }
            return Redirect(returnUrl);
        }

        [Authorize]
        [Route("[controller]/remove/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return Redirect(returnUrl);
        }

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
