using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class BlobService : IBlobService
    {
        private readonly CloudBlobClient _blobClient;

        public BlobService(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();
                //await container.SetPermissionsAsync(new BlobContainerPermissions
                //{
                //    PublicAccess = BlobContainerPublicAccessType.Blob
                //});

                var blockBlob = container.GetBlockBlobReference(fileName+".jpg");
                await blockBlob.UploadFromStreamAsync(fileStream);
                return blockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error uploading file to Blob Storage", ex);
            }
        }
    }
}
