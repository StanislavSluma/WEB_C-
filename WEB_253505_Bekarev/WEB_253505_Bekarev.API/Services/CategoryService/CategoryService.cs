using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_253505_Bekarev.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            return Task.FromResult(ResponseData<List<Category>>.Success(_appDbContext.Categories.ToList()));
        }

        public Task<ResponseData<string>> FromNormalizedName(string? NormalizedName)
        {
            if (NormalizedName == null)
                return Task.FromResult(ResponseData<string>.Success("Все"));
            var res = _appDbContext.Categories.ToList().Find(x => x.NormalizedName.Equals(NormalizedName));
            if (res == null)
                return Task.FromResult(ResponseData<string>.Error("NotFound", "Все"));
            return Task.FromResult(ResponseData<string>.Success(res.Name));
        }
    }
}
