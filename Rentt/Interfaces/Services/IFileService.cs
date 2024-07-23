namespace Rentt.Services
{
    public interface IFileService
    {
        string SaveFile(string id, IFormFile file);
        void DeleteFile(string imageUrl);
    }
}
