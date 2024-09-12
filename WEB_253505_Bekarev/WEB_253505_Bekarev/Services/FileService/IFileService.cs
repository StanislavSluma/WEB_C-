namespace WEB_253505_Bekarev.Services.FileService
{
    public interface IFileService
    {
        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="formFile">Файл, переданный формой</param>
        /// <returns>URL сохраненного файла</returns>
        Task<string> SaveFileAsync(IFormFile formFile);
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        Task DeleteFileAsync(string fileName);
        /// <summary>
        /// Изменить файл
        /// </summary>
        /// <param path="filePath">Путь к файлу</param>
        /// <param File="formFile">Файл, переданный формой</param>
        /// <returns></returns>
        Task<string> UpdateFileAsync(string filePath, IFormFile formFile);
    }
}
