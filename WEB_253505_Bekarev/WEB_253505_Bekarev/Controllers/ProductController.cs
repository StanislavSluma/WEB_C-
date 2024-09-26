using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Extensions.HostingExtensions;
using WEB_253505_Bekarev.Services.CategoryService;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Controllers
{
    [Route("Catalog/{category?}")]
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
        [HttpGet]
        public async Task<IActionResult> Index(string? category = null, int pageNo = 1)
        {
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);
            ViewData["Categories"] = categoriesResponse.Data;
            ViewData["current_category"] = (await _categoryService.FromNormalizedNameAsync(category)).Data;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductsPartial", new
                {
                    CurrentCategory = category,
                    Categories = categoriesResponse.Data,
                    Products = productResponse.Data!.Items,
                    ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
                    CurrentPage = productResponse.Data.CurrentPage,
                    TotalPages = productResponse.Data.TotalPages,
                    Admin = false
                });
            }

            return View(productResponse.Data);

        }

        // GET: ProductController/Details/5
        [HttpGet("{id:int}")]
        public ActionResult Details(int id)
        {
            return View();
        }
   
    }
}
