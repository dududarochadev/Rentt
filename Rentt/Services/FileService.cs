using Azure.Storage.Blobs;

namespace Rentt.Services
{
    public class FileService
    {
        private readonly BlobContainerClient _blobContainer;

        public FileService(IConfiguration configuration)
        {
            var azureBlob = configuration.GetValue<string>("AzureBlob");
            var azureContainer = configuration.GetValue<string>("AzureContainer");

            var blobServiceClient = new BlobServiceClient(azureBlob);
            _blobContainer = blobServiceClient.GetBlobContainerClient(azureContainer);
            _blobContainer.CreateIfNotExistsAsync();
        }

        public string SaveFile(string id, IFormFile file)
        {
            var blobClient = _blobContainer.GetBlobClient(id);

            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            blobClient.Upload(stream);

            return blobClient.Uri.AbsoluteUri;
        }

        public void DeleteFile(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            var blobClient = _blobContainer.GetBlobClient(fileName);

            blobClient.DeleteIfExists();
        }
    }
}
