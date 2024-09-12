using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_253505_Bekarev.API.Services.AnimeService
{
    public class AnimeService : IAnimeService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _appDbContext;

        public AnimeService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseData<Anime>> CreateProductAsync(Anime product)
        {

            var anime = _appDbContext.Animes.Add(product);
            await _appDbContext.SaveChangesAsync();
            return ResponseData<Anime>.Success(anime.Entity);
        }

        public async Task DeleteProductAsync(int id)
        {
            var anime = await _appDbContext.Animes.FirstOrDefaultAsync(a => a.Id == id);
            if (anime != null)
            {
                _appDbContext.Entry(anime).State = EntityState.Deleted;
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Anime>> GetProductByIdAsync(int id)
        {
            var anime = await _appDbContext.Animes.FirstOrDefaultAsync(a => a.Id == id);
            if (anime == null)
            {
                return ResponseData<Anime>.Error("Not Found", null);
            }
            return ResponseData<Anime>.Success(anime);
        }

        public Task<ResponseData<ListModel<Anime>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 5)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _appDbContext.Animes.AsQueryable();
            var dataList = new ListModel<Anime>();
            query = query.Where(d => categoryNormalizedName==null || d.Category.NormalizedName.Equals(categoryNormalizedName));
            // количество элементов в списке
            var count = query.Count(); //.Count();
            if (count==0)
            {
                return Task.FromResult(ResponseData<ListModel<Anime>>.Success(dataList));
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return Task.FromResult(ResponseData<ListModel<Anime>>.Error("No such page"));
            dataList.Items = query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return Task.FromResult(ResponseData<ListModel<Anime>>.Success(dataList));
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
        
        public async Task UpdateProductAsync(int id, Anime product)
        {
            var anime = await _appDbContext.Animes.FirstOrDefaultAsync(a => a.Id == id);
            if (anime != null)
            {
                anime.SeriesTime = product.SeriesTime;
                anime.SeriesAmount = product.SeriesAmount;
                anime.TotalTime = product.TotalTime;
                anime.Mime = product.Mime;
                anime.Name = product.Name;
                anime.Description = product.Description;
                anime.Category = product.Category;
                anime.CategoryId = product.CategoryId;
                if (product.Image is not null)
                     anime.Image = product.Image;
                _appDbContext.Entry(anime).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
