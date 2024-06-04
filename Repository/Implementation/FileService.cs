public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<Tuple<int, string>> SaveImageAsync(IFormFile imageFile, string subFolder)
    {
        try
        {
            var contentPath = Path.Combine(_environment.WebRootPath, "Uploads", subFolder);

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            // Verificar as extensões permitidas
            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtensions.Contains(ext))
            {
                string msg = string.Format("Only {0} extensions are allowed", string.Join(',', allowedExtensions));
                return new Tuple<int, string>(0, msg);
            }

            string uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(contentPath, newFileName);
            await using var stream = new FileStream(fileWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return new Tuple<int, string>(1, newFileName);
        }
        catch (Exception ex)
        {
            return new Tuple<int, string>(0, ex.Message);
        }
    }

    public async Task DeleteImageAsync(string imageFileName, string subFolder)
    {
        var contentPath = Path.Combine(_environment.WebRootPath, "Uploads", subFolder, imageFileName);

        if (File.Exists(contentPath))
        {
            await Task.Run(() => File.Delete(contentPath));
        }
    }
}
