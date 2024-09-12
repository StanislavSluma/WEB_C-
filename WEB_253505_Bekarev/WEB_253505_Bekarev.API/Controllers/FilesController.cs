using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_253505_Bekarev.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // Путь к папке wwwroot/Images
        private readonly string _imagePath;
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }
            // путь к сохраняемому файлу
            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);
            // если такой файл существует, удалить его
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            // скопировать файл в поток
            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            // получить Url файла
            // получить Url файла
            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{file.FileName}";
            return Ok(fileUrl);
        }

        [HttpDelete]
        public IActionResult DeleteFile(string file_name)
        {
            Console.WriteLine(file_name);
            string filePath = Path.Combine(_imagePath, file_name);
            var fileInfo = new FileInfo(filePath);
            // если такой файл существует, удалить его
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            return Ok();
        }
    }
}
