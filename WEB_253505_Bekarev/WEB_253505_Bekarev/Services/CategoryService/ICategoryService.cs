using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_253505_Bekarev.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();

        public Task<ResponseData<string>> FromNormalizedNameAsync(string? NormalizedName);
    }
}
