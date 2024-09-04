using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;
using WEB_253505_Bekarev.Services.CategoryService;

namespace WEB_253505_Bekarev.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<Anime> _animes;
        List<Category> _categories;
        IConfiguration _config;

        public MemoryProductService([FromServices]
                                    IConfiguration config,
                                    ICategoryService categoryService)
        {
            _config = config;
            _categories=categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }

        public Task<ResponseData<Anime>> CreateProductAsync(Anime product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Anime>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Anime>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            int page_size = int.Parse(_config["ItemsPerPage"]);

            ResponseData<ListModel<Anime>> select_animes;
            if (categoryNormalizedName == null)
                select_animes = ResponseData<ListModel<Anime>>.Success(new ListModel<Anime> { Items=_animes });
            else
            {
                var choose_animes = _animes.FindAll(x => x.Category.NormalizedName.Equals(categoryNormalizedName));
                select_animes = ResponseData<ListModel<Anime>>.Success(new ListModel<Anime> { Items=choose_animes });
            }

            select_animes.Data.TotalPages = (int)Math.Ceiling(select_animes.Data.Items.Count / (1.0 * page_size));
            select_animes.Data.CurrentPage = pageNo;

            select_animes.Data.Items = select_animes.Data.Items.Skip(page_size*(pageNo-1)).Take(page_size).ToList();

            return Task.FromResult(select_animes);
        }

        public Task UpdateProductAsync(int id, Anime product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _animes = new List<Anime>()
            {
                new Anime() 
                { 
                    Id=1, Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image="Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Id=2, Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image="Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Id=3, Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image="Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                new Anime()
                {
                    Id=4, Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image="Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Id=5, Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image="Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Id=6, Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image="Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                new Anime()
                {
                    Id=7, Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image="Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Id=8, Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image="Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Id=9, Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image="Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                new Anime()
                {
                    Id=10, Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image="Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Id=11, Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image="Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Id=12, Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image="Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
            };
        }
    }
}
