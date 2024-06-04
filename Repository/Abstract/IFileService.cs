public interface IFileService
{
    Task<Tuple<int, string>> SaveImageAsync(IFormFile imageFile, string subFolder);
    Task DeleteImageAsync(string imageFileName, string subFolder);
}
