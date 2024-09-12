using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Services.CategoryService;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _context;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var data = await _categoryService.GetCategoryListAsync();
            Categories = new SelectList(data.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Anime Anime { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }


        public SelectList Categories { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            this.Anime.TotalTime = this.Anime.SeriesAmount * this.Anime.SeriesTime;

            await _context.CreateProductAsync(Anime, Image);

            return RedirectToPage("./Index");
        }
    }
}
