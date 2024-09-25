﻿
using Microsoft.AspNetCore.Http;
using WEB_253505_Bekarev.Services.Authentication;

namespace WEB_253505_Bekarev.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;

        private readonly ITokenAccessor _tokenAccessor;
        public ApiFileService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext;

            _tokenAccessor = tokenAccessor;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            string file_name = fileName.Substring(fileName.LastIndexOf('/') + 1, fileName.Length - fileName.LastIndexOf('/') - 1);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_httpClient.BaseAddress}/?file_name={file_name}")
            };

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            await _httpClient.SendAsync(request);
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };
            // Сформировать случайное имя файла, сохранив расширение
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            // Создать контент, содержащий поток загруженного файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);
            // Поместить контент в запрос
            request.Content = content;
            // Отправить запрос к API

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }

        public async Task<string> UpdateFileAsync(string file_path, IFormFile formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            await DeleteFileAsync(file_path);
            return await SaveFileAsync(formFile);
        }
    }
}
