using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _context;

        public DetailsModel(IProductService context)
        {
            _context = context;
        }

        public Anime Anime { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Anime = (await _context.GetProductByIdAsync(id.Value)).Data;

            return Page();
        }
    }
}
