using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Services.CategoryService;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Controllers
{
    public class ProductController : Controller
    {
        ICategoryService _categoryService;
        IProductService _productService;
        public ProductController(ICategoryService category_service, IProductService product_service)
        {
            _categoryService = category_service;
            _productService = product_service;
        }
        // GET: ProductController
        public async Task<IActionResult> Index(string? category = null, int pageNo = 1)
        {
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            ViewBag.Categories = (await _categoryService.GetCategoryListAsync()).Data;
            ViewData["current_category"] = (await _categoryService.FromNormalizedNameAsync(category)).Data;
            return View(productResponse.Data);

        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
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

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
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
