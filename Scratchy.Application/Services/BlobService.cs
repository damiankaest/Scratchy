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

                // Blob-Referenz erstellen
                var blockBlob = container.GetBlockBlobReference(fileName);

                // Content-Type setzen
                blockBlob.Properties.ContentType = "image/jpeg";

                // Stream an den Anfang zurücksetzen, falls erforderlich
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }

                // Datei in den Blob Storage hochladen
                await blockBlob.UploadFromStreamAsync(fileStream);

                // URL des hochgeladenen Blobs zurückgeben
                return blockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error uploading file to Blob Storage", ex);
            }
        }
    }
}
