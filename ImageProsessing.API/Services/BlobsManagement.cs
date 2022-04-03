using ImageProsessing.API.Services.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace ImageProsessing.API.Services
{
    public class BlobsManagement: IBlobsManagement
    {
        public async Task<string> UploadFile(string containerName, string fileName, byte[] file, string connectionString){
            var container = new BlobContainerClient(connectionString,containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            var blob = container.GetBlobClient(fileName);
            Stream fileStream = new MemoryStream(file);
            await blob.UploadAsync(fileStream);
            return blob.Uri.AbsoluteUri;
        }
    }
}