namespace Scratchy.Domain.Interfaces.Services
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream);
    }
}
