using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_253505_Bekarev.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        List<Category> categories = new List<Category>{
                new Category {Id=1, Name="Фентези", NormalizedName="fantasy"},
                new Category {Id=2, Name="Детектив", NormalizedName="detective"},
                new Category {Id=3, Name="Приключения", NormalizedName="adventures"},
                new Category {Id=4, Name="Комедия", NormalizedName="comedy"},
                new Category {Id=5, Name="Повседневность", NormalizedName="everyday-life"},
                new Category {Id=6, Name="Драма", NormalizedName="drama"}
                };

        public Task<ResponseData<string>> FromNormalizedNameAsync(string? NormalizedName)
        {
            if (NormalizedName == null)
                return Task.FromResult(ResponseData<string>.Success("Все"));
            var res = categories.Find(x => x.NormalizedName.Equals(NormalizedName));
            if (res == null)
                return Task.FromResult(ResponseData<string>.Error("NotFound", "Все"));
            return Task.FromResult(ResponseData<string>.Success(res.Name));
        }

        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }


    }
}
