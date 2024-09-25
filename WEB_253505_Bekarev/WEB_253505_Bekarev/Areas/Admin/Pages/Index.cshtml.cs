﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _context;
        public ListModel<Anime> Anime { get; set; } = default!;
        public int CurrentPage;
        public int TotalPages;

        public IndexModel(IProductService context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int pageNo = 1)
        {
            Anime = (await _context.GetProductListAsync(null, pageNo)).Data;
            CurrentPage = Anime.CurrentPage;
            TotalPages = Anime.TotalPages;
        }
    }
}
