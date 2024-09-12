using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;
using WEB_253505_Bekarev.Services.FileService;

namespace WEB_253505_Bekarev.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        private readonly IFileService _fileService;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<ResponseData<ListModel<Anime>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
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
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Anime>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<ListModel<Anime>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return ResponseData<ListModel<Anime>>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteProductAsync(int id)
        {
            Anime anime = (await GetProductByIdAsync(id)).Data;
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Animes/{id}");
            await _httpClient.DeleteAsync(uri);
            await _fileService.DeleteFileAsync(anime.Image);
            return;
        }

        public async Task<ResponseData<Anime>> GetProductByIdAsync(int id)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}animes/");

            urlString.Append($"{id}");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Anime>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<Anime>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return ResponseData<Anime>.Error($"Данные не получены от сервера. Error:{response.StatusCode}", null);
        }

        public async Task<ResponseData<Anime>> CreateProductAsync(Anime product, IFormFile? formFile)
        {
            // Первоначально использовать картинку по умолчанию
            product.Image = "Images/noimage.jpg";
            // Сохранить файл изображения
            if (formFile!=null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                // Добавить в объект Url изображения
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Animes");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Anime>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode}");
            return ResponseData<Anime>.Error($"Объект не добавлен. Error:{ response.StatusCode}");
        }

        public async Task UpdateProductAsync(int id, Anime product, IFormFile? formFile)
        {
            if (formFile != null)
            {
                var imageUrl = await _fileService.UpdateFileAsync(product.Image, formFile);
                // Добавить в объект Url изображения
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }
            var uri = new Uri($"{_httpClient.BaseAddress}Animes/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
        }
    }
}
