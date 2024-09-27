using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_253505_Bekarev.BlazorWasm.Services
{
	public class DataService : IDataService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly string _pageSize;
		public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
		{
			_httpClient = httpClient;
			_pageSize = configuration.GetSection("ItemsPerPage").Value;
			_jsonSerializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_accessTokenProvider = accessTokenProvider;
		}

		public List<Category> Categories { get; set; }
		public List<Anime> Animes { get; set; }
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
		public Category? SelectedCategory { get; set; }

		public event Action DataLoaded;

		public async Task GetCategoryListAsync(int pageNo = 1)
		{
			var tokenRequest = await _accessTokenProvider.RequestAccessToken();
			if (tokenRequest.TryGetToken(out var accessToken))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Value);
			}

			var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";
			try
			{
				var response = await _httpClient.GetAsync(new Uri(urlString));
				if (!response.IsSuccessStatusCode)
				{
					Success = false;
					ErrorMessage = $"Error occured in fetching data: {response.StatusCode}";
				}

				var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>
																		(_jsonSerializerOptions);

				if (!data.Successfull)
				{
					Success = false;
					ErrorMessage = data.ErrorMessage;
				}

				Success = true;
				Categories = data.Data;
				DataLoaded.Invoke();

			}
			catch (Exception ex)
			{
				Success = false;
				ErrorMessage = $"Error occured in http client: {ex.Message}";
			}
		}

		public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
		{

			var tokenRequest = await _accessTokenProvider.RequestAccessToken();
			if (tokenRequest.TryGetToken(out var accessToken))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Value);
			}
			try
			{
				// подготовка URL запроса
				var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}animes/");

				// добавить категорию в маршрут
				if (categoryNormalizedName != null)
				{
					urlString.Append($"{categoryNormalizedName}");
				}

				// добавить номер страницы в маршрут
				if (pageNo >= 1)
				{
					urlString.Append($"?pageNo={pageNo}");
				};

				// добавить размер страницы в строку запроса
				if (!_pageSize.Equals("5"))
				{
					urlString.Append($"&pageSize={_pageSize}");
				}

				var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Anime>>>(_jsonSerializerOptions);
					if (data.Successfull)
					{
						Success = true;
						Animes = data.Data.Items;
						CurrentPage = data.Data.CurrentPage;
						TotalPages = data.Data.TotalPages;
						DataLoaded.Invoke();
					}
					else
					{
						Success = false;
						ErrorMessage = data.ErrorMessage;
					}
				}
				else 
				{
					Success = false;
					ErrorMessage = $"Error occured in fetching data: {response.StatusCode}";
				}
			}
			catch (Exception ex)
			{
				Success = false;
				ErrorMessage = $"Error occured in http client: {ex.Message}";
			}
		}
	}
}
