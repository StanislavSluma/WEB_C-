using WEB_253505_Bekarev.ClassHelpers;
using WEB_253505_Bekarev.Services.CategoryService;
using WEB_253505_Bekarev.Services.ProductService;

namespace WEB_253505_Bekarev.Extensions.HostingExtensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder, UriData uriData)
        {
            //builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            //builder.Services.AddScoped<IProductService, MemoryProductService>();
            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt=>opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
        }
    }
}
