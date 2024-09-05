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

        public Task<ResponseData<Anime>> CreateProductAsync(Anime product)
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

        public Task UpdateProductAsync(int id, Anime product)
        {
            throw new NotImplementedException();
        }
    }
}
