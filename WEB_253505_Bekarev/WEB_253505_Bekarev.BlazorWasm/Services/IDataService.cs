using WEB_253505_Bekarev.Domain.Entities;

namespace WEB_253505_Bekarev.BlazorWasm.Services
{
	public interface IDataService
	{
		event Action DataLoaded;
		List<Category> Categories { get; set; }
		List<Anime> Animes { get; set; }
		bool Success { get; set; }
		string ErrorMessage { get; set; }
		int TotalPages { get; set; }
		int CurrentPage { get; set; }
		Category SelectedCategory { get; set; }
		public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
		public Task GetCategoryListAsync(int pageNo = 1);
	}
}
